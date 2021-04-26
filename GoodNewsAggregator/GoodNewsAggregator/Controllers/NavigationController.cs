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
        private readonly IWebPageParser _parser;

        public NavigationController(IArticleService articleService, IRssService rssService, IWebPageParser parser)
        {
            _articleService = articleService;
            _rssService = rssService;
            _parser = parser;
        }

        public async Task<IActionResult> Main()
        {
            return View((await _articleService.GetAll()).ToList());
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

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            List<ArticleDto> articleDtosList = new List<ArticleDto>();
            foreach (var rss in rssSourses)
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

                if (true) //(rss.Id.Equals(new Guid("4B92ABBF-CAB0-493B-8320-857BD2901735")))
                {
                    string body;
                    Parallel.ForEach(articleDtosList, articleDto =>
                    {
                        try
                        {
                            body = _parser.Parse(articleDto.Source);
                            articleDto.Content = body;
                        }
                        catch (Exception ex)
                        {
                            articleDto.Content = "";
                            Log.Error($"Error while content page parsing. \n{ex.Message}");
                        }
                    });
                }

                news.AddRange(articleDtosList);
            }

            stopwatch.Stop();
            Log.Information($"Aggregation was executed in {stopwatch.ElapsedMilliseconds}ms and added {news.Count} articles.");

            await _articleService.AddRange(news);

            return RedirectToAction(nameof (Main));
        }
    }
}
