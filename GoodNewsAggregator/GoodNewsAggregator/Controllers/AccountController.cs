using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodNewsAggregator.DAL.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace GoodNewsAggregator.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public AccountController(ILogger<HomeController> logger, 
            UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult LogOut()
        {
            return RedirectToAction(nameof(Login));
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult MyAccount()
        {
            return View();
        }
    }
}
