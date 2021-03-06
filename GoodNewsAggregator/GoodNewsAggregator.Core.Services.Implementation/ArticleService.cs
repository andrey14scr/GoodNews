﻿using GoodNewsAggregator.Core.DTO;
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
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text.Unicode;
using System.Threading.Tasks;
using System.Xml;
using AutoMapper;
using GoodNewsAggregator.Core.Services.Implementation.Parsers;
using GoodNewsAggregator.Core.Services.Interfaces.Enums;
using GoodNewsAggregator.DAL.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using Terradue.ServiceModel.Syndication;

namespace GoodNewsAggregator.Core.Services.Implementation
{
    public class ArticleService : IArticleService
    {
        private readonly string AFINNRUJSON = "AFINN-ru.json";
        private readonly string UNKNOWNWORDSDIR = "UnknownWords";
        private readonly string UNKNOWNWORDSFILE = "words.json";

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        private readonly ConcurrentBag<(IWebPageParser Parser, Guid Id)> _parsers =
            new ConcurrentBag<(IWebPageParser parser, Guid id)>()
            {
                (new OnlinerParser(), new Guid("7EE20FB5-B62A-4DF0-A34E-2DC738D87CDE")),
                (new TjournalParser(), new Guid("95AC927C-4BA7-43E8-B408-D3B1F4C4164F")),
                (new DtfParser(), new Guid("5707D1F0-6A5C-46FB-ACEC-0288962CB53F"))
            };

        public ArticleService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
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
            var articles = await _unitOfWork.Articles.Get()
                .Where(a => a.Rss.Id.Equals(id))
                .ToListAsync();
            var articleDtos = _mapper.Map<List<ArticleDto>>(articles);

            return articleDtos;
        }

        public async Task<IEnumerable<ArticleDto>> GetFirst(int skip, int take, bool hasNulls, SortByOption sortByOption)
        {
            var result = _unitOfWork.Articles.Get()
                .Where(a => a.GoodFactor.HasValue != hasNulls);

            switch (sortByOption)
            {
                case SortByOption.DateTime:
                    result = result.OrderByDescending(a => a.Date);
                    break;
                case SortByOption.GoodFactor:
                    result = result.OrderByDescending(a => a.GoodFactor);
                    break;
            }

            return _mapper.Map<List<ArticleDto>>(await result.Skip(skip).Take(take).AsNoTracking().ToListAsync());
        }

        public async Task AggregateNews()
        {
            var rssSources = new ConcurrentBag<RssDto>(_mapper
                .Map<List<RssDto>>(await _unitOfWork.Rss.GetAll()));

            var count = 0;
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var addArticles = new ConcurrentBag<ArticleDto>();
            var updateArticles = new ConcurrentBag<ArticleDto>();
            var rssWithFeed = new ConcurrentBag<(RssDto Rss, SyndicationFeed Feed)>();

            var existList = new List<ArticleDto>();

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

            var existingArticles = new ConcurrentBag<ArticleDto>(existList);

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
                _mapper.Map<List<ArticleDto>>(await _unitOfWork.Articles.Get()
                    .Where(a => !a.GoodFactor.HasValue)
                    .Take(30)
                    .ToListAsync());

            var articleContent = new Dictionary<Guid, List<string>>();

            foreach (var item in articles)
            {
                articleContent.Add(item.Id, new List<string>());

                var text = Regex.Replace(item.Content, "<[^>]+>", " ");
                text = Regex.Replace(text, @"[\u0000-\u001F]", " ");

                var responseString = string.Empty;
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    var request = new HttpRequestMessage(HttpMethod.Post,
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

                using (var doc = JsonDocument.Parse(responseString))
                {
                    try
                    {
                        var root = doc.RootElement;
                        var arrayElement = root[0];
                        var annotationsElement = arrayElement.GetProperty("annotations");
                        var lemmaElement = annotationsElement.GetProperty("lemma");
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

            var temp = 0;
            var jsonContent = string.Empty;
            bool saveUnknownWords;
            if (!Boolean.TryParse(_configuration["Constants:SaveUnknownWords"], out saveUnknownWords))
            {
                Log.Error("Constants:SaveUnknownWords field is not valid");
                saveUnknownWords = false;
            }

            using (var sr = new StreamReader(AFINNRUJSON))
            {
                jsonContent = await sr.ReadToEndAsync();
            }

            if (string.IsNullOrWhiteSpace(jsonContent))
            {
                Log.Error("Empty afinn json file content");
                return;
            }

            var notFoundWords = new Dictionary<string, int>();

            using (var doc = JsonDocument.Parse(jsonContent))
            {
                var root = doc.RootElement;
                JsonElement valueJson;
                var valueCounter = 0; 
                var wordsCounter = 0;
                var result = 0f;

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
                        else if (saveUnknownWords && !notFoundWords.ContainsKey(word))
                        {
                            notFoundWords.Add(word, 0);
                        }
                    }

                    if (wordsCounter == 0)
                    {
                        result = 0;
                        Log.Warning("0 words found for article " + item.Key.ToString());
                    }
                    else
                        result = (float)valueCounter / wordsCounter;

                    articles[articles.FindIndex(a => a.Id == item.Key)].GoodFactor = result;
                }
            }

            await UpdateRange(articles);
            if (saveUnknownWords)
            {
                var fileName = Path.Combine(UNKNOWNWORDSDIR, UNKNOWNWORDSFILE);
                var existingItems = new Dictionary<string, int>();

                try
                {
                    if (!Directory.Exists(UNKNOWNWORDSDIR))
                        Directory.CreateDirectory(UNKNOWNWORDSDIR);
                    if (!File.Exists(fileName))
                        File.Create(fileName);

                    using (StreamReader r = new StreamReader(fileName))
                    {
                        var json = await r.ReadToEndAsync();
                        try
                        {
                            existingItems = JsonSerializer.Deserialize<Dictionary<string, int>>(json);
                        }
                        catch (Exception e)
                        {
                            Log.Error(e.Message);
                            existingItems = new Dictionary<string, int>();
                        }
                    }

                    foreach (var notFoundWord in notFoundWords)
                    {
                        if (!existingItems.ContainsKey(notFoundWord.Key))
                            existingItems.Add(notFoundWord.Key, notFoundWord.Value);
                    }

                    using (var fs = new StreamWriter(fileName, false, Encoding.UTF8))
                    {
                        var encoderSettings = new TextEncoderSettings();
                        encoderSettings.AllowRange(UnicodeRanges.All);
                        var options = new JsonSerializerOptions
                        {
                            Encoder = JavaScriptEncoder.Create(encoderSettings),
                            WriteIndented = true
                        };

                        var jsonString = JsonSerializer.Serialize(existingItems, options);
                        await fs.WriteLineAsync(jsonString);
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e.Message);
                }
            }
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
