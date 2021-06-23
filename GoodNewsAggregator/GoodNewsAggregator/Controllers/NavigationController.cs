using GoodNewsAggregator.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.Constants;
using GoodNewsAggregator.Models;
using Microsoft.AspNetCore.Authorization;

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
            var hasNulls = false;

            var news = (await _articleService.GetFirst((page - 1) * Pagination.PAGESIZE, Pagination.PAGESIZE, hasNulls)).ToList();

            int articlesCount = hasNulls ? await _articleService.GetArticlesCount() : await _articleService.GetRatedArticlesCount();

            var pageInfo = new PageInfo()
            {
                PageNumber = page,
                PageSize = Pagination.PAGESIZE,
                TotalItems = articlesCount
            };

            var articleInfos = _mapper.Map<IEnumerable<ArticleInfoViewModel>>(news).ToArray();

            for (int i = 0; i < articleInfos.Count(); i++)
            {
                articleInfos[i].RssName = (await _rssService.GetById(news[i].RssId)).Name;
            }

            return View(new NewsOnPageViewModel() { ArticleInfos = articleInfos, PageInfo = pageInfo});
        }

        public async Task<IActionResult> Article(Guid? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var article = _mapper.Map<ArticleViewModel>(await _articleService.GetById(id.Value));

            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }
    }
}
