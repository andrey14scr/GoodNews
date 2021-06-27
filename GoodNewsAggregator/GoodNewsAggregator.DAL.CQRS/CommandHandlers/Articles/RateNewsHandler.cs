using System;
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
using Microsoft.Extensions.Configuration;
using Serilog;

namespace GoodNewsAggregator.DAL.CQRS.CommandHandlers.Articles
{
    public class RateNewsHandler : IRequestHandler<RateNewsCommand, int>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        private readonly string AFINNRUJSON = "AFINN-ru.json";
        private readonly string UNKNOWNWORDSDIR = "UnknownWords";
        private readonly string UNKNOWNWORDSFILE = "words.json";

        public RateNewsHandler(GoodNewsAggregatorContext dbContext, IMapper mapper, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<int> Handle(RateNewsCommand request, CancellationToken cancellationToken)
        {
            var articles =
                _mapper.Map<List<ArticleDto>>(await _dbContext.Articles
                    .Where(a => !a.GoodFactor.HasValue).Take(30)
                    .AsNoTracking()
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

                    var httpRequest = new HttpRequestMessage(HttpMethod.Post,
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
            var jsonContent = String.Empty;
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
                return -1;
            }

            var notFoundWords = new Dictionary<string, int>();

            using (var doc = JsonDocument.Parse(jsonContent))
            {
                var root = doc.RootElement;
                JsonElement valueJson;
                var valueCounter = 0;
                var wordsCounter = 0;
                var goodFactor = 0f;

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
                        else if(saveUnknownWords && !notFoundWords.ContainsKey(word))
                        {
                            notFoundWords.Add(word, 0);
                        }
                    }

                    if (wordsCounter == 0)
                    {
                        goodFactor = 0;
                        Log.Warning("0 words found for article " + item.Key.ToString());
                    }
                    else
                        goodFactor = (float)valueCounter / wordsCounter;

                    articles[articles.FindIndex(a => a.Id == item.Key)].GoodFactor = goodFactor;
                }
            }

            _dbContext.Articles.UpdateRange(_mapper.Map<List<Article>>(articles));
            var result = await _dbContext.SaveChangesAsync(cancellationToken);

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

                    using (var r = new StreamReader(fileName))
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

            return result;
        }
    }
}