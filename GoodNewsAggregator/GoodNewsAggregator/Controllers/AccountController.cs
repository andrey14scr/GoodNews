using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.Constants;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace GoodNewsAggregator.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public AccountController(ILogger<HomeController> logger, IMapper mapper, IUserService userService)
        {
            _logger = logger;
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
            var userDto = await _userService.GetCurrentUser(HttpContext.User);
            if (userDto is null)
                return RedirectToAction(nameof(Login));

            return View(new AccountModel
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                RoleName = await _userService.GetRolesOfUser(userDto)
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                UserDto userDto = new UserDto
                {
                    Id = Guid.NewGuid(),
                    Email = registerModel.Email,
                    UserName = registerModel.UserName
                };

                var resultRegister = await _userService.Register(userDto, registerModel.Password, RoleNames.USER);

                if (resultRegister != null)
                {
                    if (resultRegister.Succeeded)
                    {
                        var resultLogin = await _userService.Login(userDto.UserName, registerModel.Password);

                        if (resultLogin.Succeeded)
                            return RedirectToAction(nameof(MyAccount));

                        return View("Error", new ErrorViewModel() 
                            { 
                            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, 
                                Message = "Ошибка во время входа в аккаунт."
                            });
                    }

                    foreach (IdentityError error in resultRegister.Errors)
                        ModelState.AddModelError("", error.Description);
                }
                else
                {
                    ModelState.AddModelError("", "Аккаунт с такой электронной почтой уже существует!");
                }
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.Login(loginModel.Username, loginModel.Password);
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
            await _userService.Logout();
            return RedirectToAction(nameof(Login));
        }

        [AcceptVerbs("GetFirst","Post")]
        public async Task<IActionResult> CheckEmail(string email)
        {
            return await _userService.Exist(email) ? Json(false) : Json(true);
        }
    }
}
