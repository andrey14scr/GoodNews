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

        public NavigationController(IArticleService dataConstructorService)
        {
            _articleService = dataConstructorService;
        }

        public IActionResult Main()
        {            
            return View(_articleService.GetRandomArticles(10).ToList());
        }

        public IActionResult Article(Guid? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            return View(_articleService.GetRandomArticles(10).ToList().Where(a => a.Id == id).FirstOrDefault());
        }
    }
}
