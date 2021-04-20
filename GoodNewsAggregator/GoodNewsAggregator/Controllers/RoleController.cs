using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GoodNewsAggregator.DAL.Core;

namespace GoodNewsAggregator.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public RoleController(IRoleService roleService, IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _roleService.GetAll());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            return await FindRole(id);
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            return await FindRole(id);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            return await FindRole(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleDto roleDto)
        {
            if (ModelState.IsValid)
            {
                roleDto.Id = Guid.NewGuid();
                await _roleService.Add(roleDto);

                return RedirectToAction(nameof(Index));
            }

            return View(roleDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, RoleDto roleDto)
        {
            if (id != roleDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _roleService.Update(roleDto);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleExists(roleDto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            return View(roleDto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var roleDto = await _roleService.GetById(id);
            _roleService.Remove(roleDto);

            return RedirectToAction(nameof(Index));
        }

        private bool RoleExists(Guid id)
        {
            return _roleService.GetById(id) != null;
        }

        private async Task<IActionResult> FindRole(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _roleService.GetById(id.Value);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }
    }
}
