using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Repositories.Interfaces;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsAggregator.Controllers
{
    public class NavigationController : Controller
    {
        private IArticleService _articleService;

        public NavigationController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        public async Task<IActionResult> MainAsync()
        {            
            return View((await _articleService.GetAll()).ToList());
        }

        public async Task<IActionResult> ArticleAsync(Guid? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            return View((await _articleService.GetAll()).Where(a => a.Id == id).FirstOrDefault());

            //return View(_articleService.GetRandomArticles(10).ToList().Where(a => a.Id == id).FirstOrDefault());
        }
    }
}
