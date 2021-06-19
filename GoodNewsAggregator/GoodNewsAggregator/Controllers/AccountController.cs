using Microsoft.AspNetCore.Mvc;

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using GoodNewsAggregator.Constants;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.Models;
using Microsoft.AspNetCore.Identity;

namespace GoodNewsAggregator.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Login(string returnUrl)
        {
            var loginModel = new LoginModel() {ReturnUrl = returnUrl };
            return View(loginModel);
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
                        var userModel = await _userService.Login(userDto.UserName, registerModel.Password);

                        if (userModel != null)
                            return RedirectToAction("Index", "Home");

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
                var userModel = await _userService.Login(loginModel.Username, loginModel.Password);
                if (userModel != null)
                {
                    if (!string.IsNullOrEmpty(loginModel.ReturnUrl) && Url.IsLocalUrl(loginModel.ReturnUrl))
                        return Redirect(loginModel.ReturnUrl);

                    return RedirectToAction("Index", "Home");
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
