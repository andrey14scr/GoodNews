using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Repositories.Interfaces;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Concurrent;
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

            double max = await result.MaxAsync(a => a.GoodFactor);
            double min = await result.MinAsync(a => a.GoodFactor);

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

            if (max == 0)
                max = 1;

            return View(new NewsOnPage() { Articles = articleDtos, PageInfo = pageInfo, MaxGoodFactor = (max + Math.Abs(min)) / 100, MinGoodFactor = Math.Abs(min) });
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
            await _articleService.AggregateNews();

            return RedirectToAction(nameof (Main));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Rate()
        {
            await _articleService.RateNews();

            return Ok();
        }
    }
}
