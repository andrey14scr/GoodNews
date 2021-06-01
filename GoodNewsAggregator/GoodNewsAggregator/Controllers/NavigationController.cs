using GoodNewsAggregator.Core.Services.Interfaces;
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

        private readonly List<(IWebPageParser Parser, Guid Id)> _parsers = new List<(IWebPageParser parser, Guid id)>()
        {
            //(new TutbyParser(), new Guid("5932E5D6-AFE4-44BF-AFD7-8BC808D66A61")),
            (new OnlinerParser(), new Guid("7EE20FB5-B62A-4DF0-A34E-2DC738D87CDE")),
            (new TjournalParser(), new Guid("95AC927C-4BA7-43E8-B408-D3B1F4C4164F")),
            //(new S13Parser(), new Guid("EC7101DA-B135-4035-ACFE-F48F1970B4CB")),
            //(new DtfParser(), new Guid("5707D1F0-6A5C-46FB-ACEC-0288962CB53F")),
        };
        
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
            rssSources = rssSources.Where(r => _parsers.Exists(p => p.Id == r.Id));
            var news = new List<ArticleDto>();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            //var tasks = rssSources.Select(async rss =>
            foreach (var rss in rssSources)
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
                    var parser = _parsers.FirstOrDefault(p => p.Id == rss.Id).Parser;

                    if (parser != null)
                    {
                        WebSiteParse(parser, ref articleDtosList);
                        news.AddRange(articleDtosList);
                    }                    
                }
            }//);
            //await Task.WhenAll(tasks);

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
