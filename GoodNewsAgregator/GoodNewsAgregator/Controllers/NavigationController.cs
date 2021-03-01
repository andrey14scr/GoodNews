using GoodNewsAgregator.Data;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsAgregator.Controllers
{
    public class NavigationController : Controller
    {
        public IActionResult Main()
        {
            List<Article> list = new List<Article>();
            list.Add(new Article() { Id = Guid.NewGuid(), Content = "blabla1", Date = DateTime.Now, GoodFactor = 0.4f, SourceId = 1, Title = "Title1" });
            list.Add(new Article() { Id = Guid.NewGuid(), Content = "blabla2", Date = DateTime.Now, GoodFactor = 0.6f, SourceId = 2, Title = "Title2" });
            list.Add(new Article() { Id = Guid.NewGuid(), Content = "blabla3", Date = DateTime.Now, GoodFactor = 0.8f, SourceId = 3, Title = "Title3" });
            return View(list);
        }

        public IActionResult Article()
        {
            return View();
        }
    }
}
