using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.DAL.Repositories.Interfaces;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using AutoMapper;
using GoodNewsAggregator.Core.Services.Implementation.Parsers;
using GoodNewsAggregator.DAL.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Terradue.ServiceModel.Syndication;

namespace GoodNewsAggregator.Core.Services.Implementation
{
    public class ArticleService : IArticleService
    {
        private readonly string AFINNRUJSON = "AFINN-ru.json";

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly ConcurrentBag<(IWebPageParser Parser, Guid Id)> _parsers =
            new ConcurrentBag<(IWebPageParser parser, Guid id)>()
            {
                (new OnlinerParser(), new Guid("7EE20FB5-B62A-4DF0-A34E-2DC738D87CDE")),
                (new TjournalParser(), new Guid("95AC927C-4BA7-43E8-B408-D3B1F4C4164F")),
                (new DtfParser(), new Guid("5707D1F0-6A5C-46FB-ACEC-0288962CB53F")),
                //(new OnlinerParser(), new Guid("0FEB39F3-5287-4A6D-ACD9-E4D27CFC69D6")),
                //(new TjournalParser(), new Guid("5A8710CF-A819-4CBB-9003-0BE2F975ABA5")),
                //(new DtfParser(), new Guid("62CFFEA0-1A14-4AC9-9CE6-4B082F029B46")),
            };

        public ArticleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Add(ArticleDto articleDto)
        {
            await AddRange(new[] { articleDto });
        }

        public async Task AddRange(IEnumerable<ArticleDto> articleDtos)
        {
            var articles = _mapper.Map<List<Article>>(articleDtos.ToList());

            await _unitOfWork.Articles.AddRange(articles);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Remove(ArticleDto articleDto)
        {
            await RemoveRange(new[] {articleDto});
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

        public async Task<IEnumerable<ArticleDto>> GetFirst(int skip, int take, bool hasNulls)
        {
            var result = await _unitOfWork.Articles
                .Get()
                .Where(a => a.GoodFactor.HasValue != hasNulls)
                .OrderByDescending(a => a.Date)
                .Skip(skip)
                .Take(take)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<List<ArticleDto>>(result);
        }

        public async Task AggregateNews()
        {
            var rssSources = new ConcurrentBag<RssDto>(
                _mapper.Map<List<RssDto>>(
                    (await _unitOfWork.Rss.GetAll()).Where(r => _parsers.Any(p => p.Id == r.Id))));

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
                await AddRange(addArticles);
                await UpdateRange(updateArticles);
            }
            catch (Exception ex)
            {
                Log.Error($"Error while articles adding. \n{ex.Message}");
            }

            stopwatch.Stop();
            Log.Information(
                $"Aggregation was executed in {stopwatch.ElapsedMilliseconds}ms and added/updated {count} articles.");
        }

        public async Task RateNews()
        {
            var articles =
                _mapper.Map<List<ArticleDto>>(await _unitOfWork.Articles.Get().Where(a => !a.GoodFactor.HasValue).Take(30)
                    .ToListAsync());

            Dictionary<Guid, List<string>> articleContent = new Dictionary<Guid, List<string>>();

            foreach (var item in articles)
            {
                articleContent.Add(item.Id, new List<string>());

                var text = Regex.Replace(item.Content, "<[^>]+>", " ");
                text = Regex.Replace(text, @"[\u0000-\u001F]", " ");

                string responseString = "";
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post,
                        "http://api.ispras.ru/texterra/v1/nlp?targetType=lemma&apikey=e4d5ecb62d3755f4845dc098adf3d25993efc96c")
                    {
                        Content = new StringContent("[{\"text\":\"" + text + "\"}]", Encoding.UTF8, "application/json")
                    };
                    var response = await httpClient.SendAsync(request);
                    responseString = await response.Content.ReadAsStringAsync();
                }

                if (string.IsNullOrWhiteSpace(responseString))
                {
                    Log.Error("Error while response getting.");
                    continue;
                }

                using (JsonDocument doc = JsonDocument.Parse(responseString))
                {
                    try
                    {
                        JsonElement root = doc.RootElement;
                        JsonElement arrayElement = root[0];
                        JsonElement annotationsElement = arrayElement.GetProperty("annotations");
                        JsonElement lemmaElement = annotationsElement.GetProperty("lemma");
                        JsonElement valueJson;

                        foreach (var element in lemmaElement.EnumerateArray())
                        {
                            if (element.TryGetProperty("value", out valueJson))
                            {
                                string valueString = valueJson.ToString();
                                if (!string.IsNullOrWhiteSpace(valueString))
                                    articleContent[item.Id].Add(valueString);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Error while adding words from article. More:\n" + ex.Message);
                    }
                }

            }

            int temp = 0;

            string jsonContent = "";

            using (var sr = new StreamReader(AFINNRUJSON))
            {
                jsonContent = await sr.ReadToEndAsync();
            }

            if (string.IsNullOrWhiteSpace(jsonContent))
            {
                Log.Error("Empty afinn json file content");
                return;
            }

            using (JsonDocument doc = JsonDocument.Parse(jsonContent))
            {
                JsonElement root = doc.RootElement;
                JsonElement valueJson;
                int valueCounter, wordsCounter;
                float result = 0;

                foreach (var item in articleContent)
                {
                    valueCounter = 0;
                    wordsCounter = 0;

                    foreach (var word in item.Value)
                    {
                        if (root.TryGetProperty(word, out valueJson) && valueJson.TryGetInt32(out temp))
                        {
                            valueCounter += temp;
                            wordsCounter++;
                            
                            temp = 0;
                        }
                    }

                    if (wordsCounter == 0)
                        result = 0;
                    else
                        result = (float)valueCounter / wordsCounter;

                    articles[articles.FindIndex(a => a.Id == item.Key)].GoodFactor = result;
                }
            }

            await UpdateRange(articles);
        }

        public async Task<int> GetArticlesCount()
        {
            return await _unitOfWork.Articles.Get().CountAsync();
        }

        public async Task<int> GetRatedArticlesCount()
        {
            return await _unitOfWork.Articles.Get().Where(a => a.GoodFactor.HasValue).CountAsync();
        }
    }
}
