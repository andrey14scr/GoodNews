using Microsoft.AspNetCore.Mvc;

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.Models;
using Microsoft.AspNetCore.Identity;
using Serilog;

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
                var userDto = new UserDto()
                {
                    Id = Guid.NewGuid(),
                    Email = registerModel.Email,
                    Role = Constants.RoleNames.USER,
                    UserName = registerModel.UserName
                };

                try
                {
                    var resultRegister = await _userService.Register(userDto, registerModel.Password);
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
                        ModelState.AddModelError(string.Empty, error.Description);
                }
                catch (Exception e)
                {
                    Log.Error(e.Message);
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
                var userModel = await _userService.Login(loginModel.UserName, loginModel.Password);
                if (userModel != null)
                {
                    if (!string.IsNullOrEmpty(loginModel.ReturnUrl) && Url.IsLocalUrl(loginModel.ReturnUrl))
                        return Redirect(loginModel.ReturnUrl);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Неправильный логин и (или) пароль");
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

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> CheckEmail(string email)
        {
            return (await _userService.GetByEmail(email)) != null
                ? Json(false)
                : Json(true);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> CheckUserName(string userName)
        {
            return (await _userService.GetByUserName(userName)) != null
                ? Json(false)
                : Json(true);
        }
    }
}
