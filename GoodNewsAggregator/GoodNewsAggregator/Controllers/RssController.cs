using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.Core.DTO;

namespace GoodNewsAggregator.Controllers
{
    public class RssController : Controller
    {
        //private readonly GoodNewsAggregatorContext _context;
        private readonly IRssService _rssService;

        public RssController(IRssService rssService)
        {
            //_context = context;
            _rssService = rssService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _rssService.GetAll());
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            return await FindRss(id);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            return await FindRss(id);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            return await FindRss(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RssDto rss)
        {
            if (ModelState.IsValid)
            {
                rss.Id = Guid.NewGuid();
                await _rssService.Add(rss);
                //await _context.SaveChangesAsync(); // !

                return RedirectToAction(nameof(Index));
            }

            return View(rss);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, RssDto rss)
        {
            if (id != rss.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _rssService.Update(rss);
                    //await _context.SaveChangesAsync(); // !
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await RssExistsAsync(rss.Id)))
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

            return View(rss);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var rss = await _rssService.GetById(id);
            await _rssService.Delete(rss);
            //await _context.SaveChangesAsync(); // !

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> RssExistsAsync(Guid id) // any !
        {
            return await _rssService.GetById(id) != null;
        }

        private async Task<IActionResult> FindRss(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rss = await _rssService.GetById(id.Value);
            if (rss == null)
            {
                return NotFound();
            }

            return View(rss);
        }
    }
}
