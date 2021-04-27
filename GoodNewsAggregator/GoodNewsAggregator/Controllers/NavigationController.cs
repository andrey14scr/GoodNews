using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Repositories.Interfaces;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Implementation;
using GoodNewsAggregator.Models;
using GoodNewsAggregator.Views.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public NavigationController(IArticleService articleService, IRssService rssService)
        {
            _articleService = articleService;
            _rssService = rssService;
        }

        public async Task<IActionResult> Main(int pageNumber = 1)
        {
            var news = (await _articleService.GetAll()).ToList();

            var pageSize = Pagination.PAGESIZE;

            var newsPerPages = news.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            var pageInfo = new PageInfo()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = news.Count
            };

            return View(new NewsWithPages() { Articles = newsPerPages, PageInfo = pageInfo });
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

        public async Task<IActionResult> Aggregate()
        {
            var rssSourses = await _rssService.GetAll();
            var news = new List<ArticleDto>();
            var listExceptions = new List<Guid>();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            List<ArticleDto> articleDtosList = new List<ArticleDto>();
            var tasks = rssSourses.Select(async rss =>
            {
                try
                {
                    articleDtosList = (List<ArticleDto>) await _articleService.GetArticleInfoFromRss(rss);
                }
                catch (Exception ex)
                {
                    articleDtosList.Clear();
                    Log.Error($"Error while rss parsing. \n{ex.Message}");
                }

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
            });
            await Task.WhenAll(tasks);

            stopwatch.Stop();
            Log.Information($"Aggregation was executed in {stopwatch.ElapsedMilliseconds}ms and added {news.Count} articles.");

            news = news.Where(a => !listExceptions.Contains(a.Id)).ToList();

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
