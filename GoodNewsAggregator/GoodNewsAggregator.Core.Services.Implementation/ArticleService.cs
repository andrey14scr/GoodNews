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
            //(new TutbyParser(), new Guid("5932E5D6-AFE4-44BF-AFD7-8BC808D66A61")),
            (new OnlinerParser(), new Guid("7EE20FB5-B62A-4DF0-A34E-2DC738D87CDE")),
            (new TjournalParser(), new Guid("95AC927C-4BA7-43E8-B408-D3B1F4C4164F")),
            //(new S13Parser(), new Guid("EC7101DA-B135-4035-ACFE-F48F1970B4CB")),
            (new DtfParser(), new Guid("5707D1F0-6A5C-46FB-ACEC-0288962CB53F")),
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
            await _unitOfWork.Articles.RemoveRange(articles);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Update(ArticleDto articleDto)
        {
            var article = _mapper.Map<Article>(articleDto);
            await _unitOfWork.Articles.Update(article);
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task UpdateRange(IEnumerable<ArticleDto> articleDtos)
        {
            var articles = _mapper.Map<List<Article>>(articleDtos);
            await _unitOfWork.Articles.UpdateRange(articles);
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

            foreach (var rss in rssSources)
            {
                try
                {
                    var parser = _parsers.FirstOrDefault(p => p.Id == rss.Id).Parser;

                    using (var reader = XmlReader.Create(rss.Url))
                    {
                        var feed = SyndicationFeed.Load(reader);
                        reader.Close();

                        if (feed.Items.Any())
                        {
                            count += feed.Items.Count();

                            var urls = feed.Items.Select(f => f.Links[0].Uri.ToString());

                            var existList = _mapper.Map<List<ArticleDto>>(
                                await _unitOfWork.Articles.Get()
                                .Where(a => urls.Any(f => f == a.Source))
                                .ToListAsync()
                                );

                            ConcurrentBag<ArticleDto> existingArticles = new ConcurrentBag<ArticleDto>(existList);

                            Parallel.ForEach(feed.Items, syndicationItem =>
                            {
                                var uri = syndicationItem.Links[0].Uri.ToString();

                                var existingArticle = existingArticles.FirstOrDefault(a => a.Source == uri);

                                if (existingArticle == null)
                                {
                                    var newsDto = new ArticleDto()
                                    {
                                        Id = Guid.NewGuid(),
                                        RssId = rss.Id,
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
                }
                catch (Exception ex)
                {
                    Log.Error($"Error while rss parsing. \n{ex.Message}");
                }
            }

            await AddRange(addArticles);
            await UpdateRange(updateArticles);

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
