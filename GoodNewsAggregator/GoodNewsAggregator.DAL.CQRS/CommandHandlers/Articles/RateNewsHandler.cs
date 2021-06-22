﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text.Unicode;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.CQRS.Commands.Articles;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace GoodNewsAggregator.DAL.CQRS.CommandHandlers.Articles
{
    public class RateNewsHandler : IRequestHandler<RateNewsCommand, int>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        private readonly string AFINNRUJSON = "AFINN-ru.json";
        private readonly string UNKNOWNWORDSDIR = "UnknownWords";

        public RateNewsHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(RateNewsCommand request, CancellationToken cancellationToken)
        {
            var articles =
                _mapper.Map<List<ArticleDto>>(await _dbContext.Articles
                    .Where(a => !a.GoodFactor.HasValue).Take(30)
                    .AsNoTracking()
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

                    HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Post,
                        "http://api.ispras.ru/texterra/v1/nlp?targetType=lemma&apikey=e4d5ecb62d3755f4845dc098adf3d25993efc96c")
                    {
                        Content = new StringContent("[{\"text\":\"" + text + "\"}]", Encoding.UTF8, "application/json")
                    };
                    var httpResponse = await httpClient.SendAsync(httpRequest);
                    responseString = await httpResponse.Content.ReadAsStringAsync();
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

            string jsonContent = String.Empty;

            using (var sr = new StreamReader(AFINNRUJSON))
            {
                jsonContent = await sr.ReadToEndAsync();
            }

            if (string.IsNullOrWhiteSpace(jsonContent))
            {
                Log.Error("Empty afinn json file content");
                return -1;
            }

            Dictionary<string, int> notFoundWords = new Dictionary<string, int>();

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
                        else if(!notFoundWords.ContainsKey(word))
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

            _dbContext.Articles.UpdateRange(_mapper.Map<List<Article>>(articles));

            try
            {
                if (!Directory.Exists(UNKNOWNWORDSDIR))
                {
                    Directory.CreateDirectory(UNKNOWNWORDSDIR);
                }
                string fileName = Path.Combine(UNKNOWNWORDSDIR, DateTime.Now.ToString("yyyy-MM-dd-HH-mm") + "-words.json");
                using (var fs = new StreamWriter(fileName, false, Encoding.UTF8))
                {
                    var encoderSettings = new TextEncoderSettings();
                    encoderSettings.AllowRange(UnicodeRanges.All);
                    var options = new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.Create(encoderSettings),
                        WriteIndented = true
                    };

                    string jsonString = JsonSerializer.Serialize(notFoundWords, options);
                    await fs.WriteLineAsync(jsonString);
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }

            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}