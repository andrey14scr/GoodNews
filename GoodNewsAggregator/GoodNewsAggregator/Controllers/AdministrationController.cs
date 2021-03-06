﻿using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.Core.Services.Interfaces.Constants;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace GoodNewsAggregator.Controllers
{
    [Authorize(Roles = RoleNames.ADMIN)]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly IArticleService _articleService;
        private readonly IMapper _mapper;

        public AdministrationController(RoleManager<Role> roleManager, IMapper mapper, IArticleService articleService)
        {
            _roleManager = roleManager;
            _mapper = mapper;
            _articleService = articleService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await GetAdminInfo());
        }

        [HttpPost]
        public async Task<IActionResult> Aggregate()
        {
            await _articleService.AggregateNews();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Rate()
        {
            await _articleService.RateNews();

            return RedirectToAction("Index");
        }

        public IActionResult Roles()
        {
            return View(_mapper.Map<IEnumerable<RoleDto>>(_roleManager.Roles) );
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

        private async Task<AdminInfo> GetAdminInfo()
        {
            var count = await _articleService.GetArticlesCount();
            var rated = await _articleService.GetRatedArticlesCount();

            return new AdminInfo() { ArticleCount = count, RatedArticles = rated };
        }
    }
}
