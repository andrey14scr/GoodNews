﻿using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Repositories.Interfaces;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.Constants;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Implementation.Parsers;
using GoodNewsAggregator.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace GoodNewsAggregator.Controllers
{
    [Authorize]
    public class NavigationController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly IRssService _rssService;
        private readonly TutbyParser _tutbyParser = new TutbyParser();
        private readonly OnlinerParser _onlinerParser = new OnlinerParser();
        private readonly TjournalParser _tjournalParser = new TjournalParser();
        private readonly S13Parser _s13Parser = new S13Parser();
        private readonly DtfParser _dtfParser = new DtfParser();
        private readonly IMapper _mapper;

        public NavigationController(IArticleService articleService, IRssService rssService, IMapper mapper)
        {
            _articleService = articleService;
            _rssService = rssService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Main(int page = 1)
        {
            var result = _articleService.Get();
            int articlesCount = result.Count();

            result = result.OrderByDescending(a => a.Date)
                .Skip((page - 1) * Pagination.PAGESIZE)
                .Take(Pagination.PAGESIZE);
            var news = await result.ToListAsync();

            var pageInfo = new PageInfo()
            {
                PageNumber = page,
                PageSize = Pagination.PAGESIZE,
                TotalItems = articlesCount
            };

            var articleDtos = _mapper.Map<IEnumerable<ArticleWithRssNameDto>>(news);

            foreach (var article in articleDtos)
            {
                article.RssName = (await _rssService.GetById(article.RssId)).Name;
            }

            return View(new NewsOnPage() { Articles = articleDtos, PageInfo = pageInfo });
        }

        public async Task<IActionResult> Article(Guid? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var article = await _articleService.GetById(id.Value);

            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Aggregate()
        {
            var rssSources = await _rssService.GetAll();
            var news = new List<ArticleDto>();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var tasks = rssSources.Select(async rss =>
            {
                var articleDtosList = new List<ArticleDto>();
                try
                {
                    articleDtosList = (List<ArticleDto>) await _articleService.GetArticleInfosFromRss(rss);
                }
                catch (Exception ex)
                {
                    articleDtosList.Clear();
                    Log.Error($"Error while rss parsing. \n{ex.Message}");
                }

                if (articleDtosList.Count != 0)
                {
                    if (rss.Id.Equals(new Guid("0feb39f3-5287-4a6d-acd9-e4d27cfc69d6"))) //onliner
                        WebSiteParse(_onlinerParser, ref articleDtosList);
                    else if (rss.Id.Equals(new Guid("8d96b48b-1b3d-4981-9595-18b526bd93b6"))) //tutby
                        WebSiteParse(_tutbyParser, ref articleDtosList);
                    else if (rss.Id.Equals(new Guid("5a8710cf-a819-4cbb-9003-0be2f975aba5"))) //tjournal
                        WebSiteParse(_tjournalParser, ref articleDtosList);
                    else if (rss.Id.Equals(new Guid("c288fe8e-ca4d-482a-baa1-bf2ff7244726"))) //s13
                        WebSiteParse(_s13Parser, ref articleDtosList);
                    else if (rss.Id.Equals(new Guid("62cffea0-1a14-4ac9-9ce6-4b082f029b46"))) //dtf
                        WebSiteParse(_dtfParser, ref articleDtosList);

                    news.AddRange(articleDtosList);
                }
            });
            await Task.WhenAll(tasks);

            stopwatch.Stop();
            Log.Information($"Aggregation was executed in {stopwatch.ElapsedMilliseconds}ms and added {news.Count} articles.");

            await _articleService.AddRange(news);

            return RedirectToAction(nameof (Main));
        }

        private static void WebSiteParse(IWebPageParser parser, ref List<ArticleDto> articleDtos)
        {
            string body;

            Parallel.ForEach(articleDtos, articleDto =>
            {
                body = parser.Parse(articleDto.Source);

                if (!string.IsNullOrEmpty(body))
                    articleDto.Content = body;
            });
        }
    }
}
