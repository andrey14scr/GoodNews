using GoodNewsAgregator.Data;
using GoodNewsAgregator.Services;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsAgregator.Controllers
{
    public class NavigationController : Controller
    {
        private IDataConstructorService _dataConstructorService;

        public NavigationController(IDataConstructorService dataConstructorService)
        {
            _dataConstructorService = dataConstructorService;
        }

        public IActionResult Main()
        {
            List<Article> list = _dataConstructorService.GetArticles(10).ToList();
            
            return View(list);
        }

        public IActionResult Article(Guid? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            return View(_dataConstructorService.GetArticles(10).ToList().Where(a => a.Id == id).FirstOrDefault());
        }
    }
}
