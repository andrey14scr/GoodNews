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


            return RedirectToAction(nameof (Main));
        }
    }
}
