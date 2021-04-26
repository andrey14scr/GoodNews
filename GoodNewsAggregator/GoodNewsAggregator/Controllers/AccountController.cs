using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.Models;
using GoodNewsAggregator.Views.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace GoodNewsAggregator.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public AccountController(ILogger<HomeController> logger, 
            UserManager<User> userManager, RoleManager<Role> roleManager, SignInManager<User> signInManager,
            IMapper mapper, IUserService userService)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _userService = userService;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public async Task<IActionResult> MyAccount()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user is null)
            {
                return RedirectToAction(nameof(Login));
            }

            return View(new AccountModel
            {
                UserName = user.UserName, 
                Email = user.Email,
                RoleName = (await _userManager.GetRolesAsync(user)).Aggregate((a, b) => a + ", " + b)
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    Id = Guid.NewGuid(),
                    Email = registerModel.Email,
                    UserName = registerModel.UserName
                };

                IdentityResult result = await _userManager.CreateAsync(user, registerModel.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);

                    if(_userManager.Users.Count() == 1)
                        await _userManager.AddToRoleAsync(user, RoleNames.ADMIN);
                    else
                        await _userManager.AddToRoleAsync(user, RoleNames.USER);

                    return RedirectToAction(nameof(MyAccount));
                }

                foreach (IdentityError error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginModel.Username, loginModel.Password, false, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(loginModel.ReturnUrl) && Url.IsLocalUrl(loginModel.ReturnUrl))
                        return Redirect(loginModel.ReturnUrl);

                    return RedirectToAction(nameof(MyAccount));
                }

                ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }
    }
}
