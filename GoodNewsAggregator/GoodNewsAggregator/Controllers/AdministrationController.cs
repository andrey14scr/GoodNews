using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.DAL.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace GoodNewsAggregator.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministrationController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public AdministrationController(ILogger<HomeController> logger,
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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Roles()
        {
            return View(_mapper.Map<IEnumerable<RoleDto>>(_roleManager.Roles) );
        }

        public IActionResult Users()
        {
            return View();
        }

        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleDto roleDto)
        {
            await _roleManager.CreateAsync(new Role(){Name = roleDto .Name});
            return RedirectToAction(nameof(Roles));
        }

        public async Task<IActionResult> DeleteRole(Guid id)
        {
            await _roleManager.DeleteAsync(await _roleManager.FindByIdAsync(id.ToString()));
            return RedirectToAction(nameof(Roles));
        }
    }
}
