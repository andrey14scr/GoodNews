using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using AutoMapper;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Implementation.Parsers;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.CQRS.Commands.Articles;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Terradue.ServiceModel.Syndication;

namespace GoodNewsAggregator.DAL.CQRS.CommandHandlers.Articles
{
    public class AggregateNewsHandler : IRequestHandler<AggregateNewsCommand, int>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        private readonly ConcurrentBag<(IWebPageParser Parser, Guid Id)> _parsers =
            new ConcurrentBag<(IWebPageParser parser, Guid id)>()
            {
                (new OnlinerParser(), new Guid("7EE20FB5-B62A-4DF0-A34E-2DC738D87CDE")),
                (new TjournalParser(), new Guid("95AC927C-4BA7-43E8-B408-D3B1F4C4164F")),
                (new DtfParser(), new Guid("5707D1F0-6A5C-46FB-ACEC-0288962CB53F")),
            };

        public AggregateNewsHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(AggregateNewsCommand request, CancellationToken cancellationToken)
        {
            var rssSources = new ConcurrentBag<RssDto>(
                _mapper.Map<List<RssDto>>(await _dbContext.Rss
                .AsNoTracking()
                .ToListAsync()));

            int count = 0;
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            ConcurrentBag<ArticleDto> addArticles = new ConcurrentBag<ArticleDto>();
            ConcurrentBag<ArticleDto> updateArticles = new ConcurrentBag<ArticleDto>();
            ConcurrentBag<(RssDto Rss, SyndicationFeed Feed)> rssWithFeed =
                new ConcurrentBag<(RssDto Rss, SyndicationFeed feed)>();

            List<ArticleDto> existList = new List<ArticleDto>();

            foreach (var rss in rssSources)
            {
                using (var reader = XmlReader.Create(rss.Url))
                {
                    var feed = SyndicationFeed.Load(reader);
                    rssWithFeed.Add((rss, feed));
                    reader.Close();

                    var urls = feed.Items.Select(f => f.Links[0].Uri.ToString());

                    existList.AddRange(_mapper.Map<List<ArticleDto>>(await _dbContext.Articles
                        .Where(a => urls.Any(f => f == a.Source))
                        .AsNoTracking()
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
                                    GoodFactor = null
                                };

                                addArticles.Add(newsDto);
                            }
                            else if (existingArticle.Date != syndicationItem.PublishDate.DateTime)
                            {
                                existingArticle.Content = parser.Parse(uri);
                                existingArticle.Date = syndicationItem.PublishDate.DateTime;
                                existingArticle.Title = syndicationItem.Title.Text;
                                existingArticle.GoodFactor = null;

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
                _dbContext.Articles.AddRange(_mapper.Map<IEnumerable<Article>>(addArticles));
                _dbContext.Articles.UpdateRange(_mapper.Map<IEnumerable<Article>>(updateArticles));
            }
            catch (Exception ex)
            {
                Log.Error($"Error while articles adding. \n{ex.Message}");
            }

            stopwatch.Stop();
            Log.Information(
                $"Aggregation was executed in {stopwatch.ElapsedMilliseconds}ms and added/updated {count} articles.");

            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}