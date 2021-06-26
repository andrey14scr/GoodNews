using GoodNewsAggregator.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.Core.Services.Interfaces.Enums;
using GoodNewsAggregator.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace GoodNewsAggregator.Controllers
{
    [Authorize]
    public class NavigationController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly IRssService _rssService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public NavigationController(IArticleService articleService, IRssService rssService, IMapper mapper, IConfiguration configuration)
        {
            _articleService = articleService;
            _rssService = rssService;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<IActionResult> Main(int page = 1, SortByOption sortBy = SortByOption.DateTime)
        {
            var hasNulls = false;

            int pageSize = 0;
            if (!Int32.TryParse(_configuration["Constants:PageSize"], out pageSize))
            {
                Log.Error("Constants:PageSize field is not valid");
                pageSize = 20;
            }

            var news = (await _articleService.GetFirst((page - 1) * pageSize, pageSize, hasNulls, sortBy)).ToList();

            int articlesCount = hasNulls ? await _articleService.GetArticlesCount() : await _articleService.GetRatedArticlesCount();

            var pageInfo = new PageInfo()
            {
                PageNumber = page,
                PageSize = pageSize,
                TotalItems = articlesCount, 
                SortByOption = sortBy
            };

            var articleInfos = _mapper.Map<IEnumerable<ArticleInfoViewModel>>(news).ToArray();

            for (int i = 0; i < articleInfos.Count(); i++)
            {
                articleInfos[i].RssName = (await _rssService.GetById(news[i].RssId)).Name;
            }

            return View(new NewsOnPageViewModel() { ArticleInfos = articleInfos, PageInfo = pageInfo });
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
