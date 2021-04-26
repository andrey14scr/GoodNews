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
        private IArticleService _articleService;
        private readonly IRssService _rssService;

        public NavigationController(IArticleService articleService, IRssService rssService)
        {
            _articleService = articleService;
            _rssService = rssService;
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
                articleDtosList = (List<ArticleDto>) await _articleService.GetArticleInfoFromRss(rss);

                if (rss.Id.Equals(new Guid("4B92ABBF-CAB0-493B-8320-857BD2901735")))
                {
                    foreach (var articleDto in articleDtosList)
                    {
                        //var body = await _onlinerParser.Parse(articleDto.Source);
                        //articleDto.Content = body;
                    }
                }

                news.AddRange(articleDtosList);
            }

            await _articleService.AddRange(news);

            stopwatch.Stop();
            Log.Information($"Aggregation was executed in {stopwatch.ElapsedMilliseconds}");

            return RedirectToAction(nameof (Main));
        }
    }
}
