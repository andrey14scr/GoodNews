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
            return View();
        }

        public IActionResult Article()
        {
            return View();
        }
    }
}
