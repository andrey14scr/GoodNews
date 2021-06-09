using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Repositories.Implementation;
using GoodNewsAggregator.DAL.Repositories.Interfaces;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using AutoMapper;
using GoodNewsAggregator.Core.Services.Implementation.Parsers;
using GoodNewsAggregator.DAL.Core.Entities;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Terradue.ServiceModel.Syndication;

namespace GoodNewsAggregator.Core.Services.Implementation
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly ConcurrentBag<(IWebPageParser Parser, Guid Id)> _parsers = new ConcurrentBag<(IWebPageParser parser, Guid id)>()
        {
            (new OnlinerParser(), new Guid("0FEB39F3-5287-4A6D-ACD9-E4D27CFC69D6")),
            (new TjournalParser(), new Guid("5A8710CF-A819-4CBB-9003-0BE2F975ABA5")),
            (new DtfParser(), new Guid("62CFFEA0-1A14-4AC9-9CE6-4B082F029B46")),
        };
        public ArticleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Add(ArticleDto commentDto)
        {
            await AddRange(new[] { commentDto });
        }

        public async Task AddRange(IEnumerable<ArticleDto> articleDtos)
        {
            var articles = _mapper.Map<List<Article>>(articleDtos.ToList());

            await _unitOfWork.Articles.AddRange(articles);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Remove(ArticleDto articleDto)
        {
            await RemoveRange(new[] { articleDto });
        }

        public async Task RemoveRange(IEnumerable<ArticleDto> articleDtos)
        {
            var articles = _mapper.Map<List<Article>>(articleDtos.ToList());
            _unitOfWork.Articles.RemoveRange(articles);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Update(ArticleDto articleDto)
        {
            var article = _mapper.Map<Article>(articleDto);
            _unitOfWork.Articles.Update(article);
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task UpdateRange(IEnumerable<ArticleDto> articleDtos)
        {
            var articles = _mapper.Map<List<Article>>(articleDtos);
            _unitOfWork.Articles.UpdateRange(articles);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<ArticleDto>> GetAll()
        {
            var articles = await _unitOfWork.Articles.GetAll();
            var articleDtos = _mapper.Map<List<ArticleDto>>(articles);

            return articleDtos;
        }

        public async Task<ArticleDto> GetById(Guid id)
        {
            var article = await _unitOfWork.Articles.GetById(id);
            var articleDto = _mapper.Map<ArticleDto>(article);

            return articleDto;
        }

        public async Task<IEnumerable<ArticleDto>> GetByRssId(Guid id)
        {
            var articles = await _unitOfWork.Articles.Get().Where(a => a.Rss.Id.Equals(id)).ToListAsync();
            var articleDtos = _mapper.Map<List<ArticleDto>>(articles);

            return articleDtos;
        }

        public async Task AggregateNews()
        {
            var rssSources = new ConcurrentBag<RssDto>(_mapper.Map<List<RssDto>>((await _unitOfWork.Rss.GetAll()).Where(r => _parsers.Any(p => p.Id == r.Id))));

            int count = 0;
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            ConcurrentBag<ArticleDto> addArticles = new ConcurrentBag<ArticleDto>();
            ConcurrentBag<ArticleDto> updateArticles = new ConcurrentBag<ArticleDto>();
            ConcurrentBag<(RssDto Rss, SyndicationFeed Feed)> rssWithFeed = new ConcurrentBag<(RssDto Rss, SyndicationFeed feed)>();

            List<ArticleDto> existList = new List<ArticleDto>();

            foreach (var rss in rssSources)
            {
                using (var reader = XmlReader.Create(rss.Url))
                {
                    var feed = SyndicationFeed.Load(reader);
                    rssWithFeed.Add((rss, feed));
                    reader.Close();

                    var urls = feed.Items.Select(f => f.Links[0].Uri.ToString());

                    existList.AddRange(_mapper.Map<List<ArticleDto>>(await _unitOfWork.Articles.Get()
                            .Where(a => urls.Any(f => f == a.Source))
                            .ToListAsync())
                    );
                }
            }

            ConcurrentBag<ArticleDto> existingArticles = new ConcurrentBag<ArticleDto>(existList);

            Parallel.ForEach(rssWithFeed, rf =>
            {
                try
                {
                    var parser = _parsers.FirstOrDefault(p => p.Id == rf.Rss.Id).Parser;

                    if (rf.Feed.Items.Any())
                    {
                        count += rf.Feed.Items.Count();

                        Parallel.ForEach(rf.Feed.Items, syndicationItem =>
                        {
                            var uri = syndicationItem.Links[0].Uri.ToString();

                            var existingArticle = existingArticles.FirstOrDefault(a => a.Source == uri);

                            if (existingArticle == null)
                            {
                                var newsDto = new ArticleDto()
                                {
                                    Id = Guid.NewGuid(),
                                    RssId = rf.Rss.Id,
                                    Source = uri,
                                    Title = syndicationItem.Title.Text,
                                    Date = syndicationItem.PublishDate.DateTime,
                                    Content = parser.Parse(uri),
                                    GoodFactor = 0
                                };

                                addArticles.Add(newsDto);
                            }
                            else if (existingArticle.Date != syndicationItem.PublishDate.DateTime)
                            {
                                existingArticle.Content = parser.Parse(uri);
                                existingArticle.Date = syndicationItem.PublishDate.DateTime;
                                existingArticle.Title = syndicationItem.Title.Text;
                                existingArticle.GoodFactor = 0;

                                updateArticles.Add(existingArticle);
                            }
                        });
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while rss parsing. \n{ex.Message}");
                }
            });

            try
            {
                await AddRange(addArticles);
                await UpdateRange(updateArticles);
            }
            catch (Exception ex)
            {
                Log.Error($"Error while articles adding. \n{ex.Message}");
            }

            stopwatch.Stop();
            Log.Information($"Aggregation was executed in {stopwatch.ElapsedMilliseconds}ms and added/updated {count} articles.");
        }

        public async Task RateNews()
        {

        }

        public IQueryable<Article> Get()
        {
            return _unitOfWork.Articles.Get();
        }
    }
}
