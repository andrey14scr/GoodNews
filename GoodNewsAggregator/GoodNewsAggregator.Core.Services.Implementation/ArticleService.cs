﻿using GoodNewsAggregator.Core.DTO;
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

        private readonly List<(IWebPageParser Parser, Guid Id)> _parsers = new List<(IWebPageParser parser, Guid id)>()
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

        public async Task<IEnumerable<ArticleDto>> GetArticleInfosFromRss(RssDto rss, IWebPageParser parser)
        {
            var articleDtos = new ConcurrentBag<ArticleDto>();

            using (var reader = XmlReader.Create(rss.Url))
            {
                var feed = SyndicationFeed.Load(reader);
                reader.Close();
                
                if (feed.Items.Any())
                {
                    var urls = await _unitOfWork.Articles
                        .Get()
                        .Select(a => a.Source)
                        .ToListAsync();

                    ConcurrentBag<string> currentArticleUrls = new ConcurrentBag<string>(urls);

                    Parallel.ForEach(feed.Items, syndicationItem =>
                    {
                        if (!currentArticleUrls.Any(url => url.Equals(syndicationItem.Links[0].Uri.ToString())))
                        {
                            var newsDto = new ArticleDto()
                            {
                                Id = Guid.NewGuid(),
                                RssId = rss.Id,
                                Source = syndicationItem.Links[0].Uri.ToString(),
                                Title = syndicationItem.Title.Text,
                                Date = syndicationItem.PublishDate.DateTime, 
                                Content = parser.Parse(syndicationItem.Links[0].Uri.ToString()), 
                                GoodFactor = 0
                            };

                            articleDtos.Add(newsDto);
                        }
                    });
                }
            }

            return articleDtos;
        }

        public async Task AggregateNews()
        {
            var rssSources = _mapper.Map<List<RssDto>>((await _unitOfWork.Rss.GetAll()).Where( r => _parsers.Exists(p => p.Id == r.Id)));

            int count = 0;
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            foreach (var rss in rssSources)
            {
                var articleDtosList = new ConcurrentBag<ArticleDto>();
                try
                {
                    articleDtosList = (ConcurrentBag<ArticleDto>)await this.GetArticleInfosFromRss(rss, _parsers.FirstOrDefault(p => p.Id == rss.Id).Parser);
                    await this.AddRange(articleDtosList);
                }
                catch (Exception ex)
                {
                    articleDtosList.Clear();
                    Log.Error($"Error while rss parsing. \n{ex.Message}");
                }
            }

            stopwatch.Stop();
            Log.Information($"Aggregation was executed in {stopwatch.ElapsedMilliseconds}ms and added {count} articles.");
        }

        public IQueryable<Article> Get()
        {
            return _unitOfWork.Articles.Get();
        }
    }
}
