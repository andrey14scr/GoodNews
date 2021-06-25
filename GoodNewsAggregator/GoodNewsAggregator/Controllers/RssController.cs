using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.Core.DTO;
using Microsoft.AspNetCore.Authorization;

namespace GoodNewsAggregator.Controllers
{
    [Authorize(Roles = "Admin,Moderator")]
    public class RssController : Controller
    {
        private readonly IRssService _rssService;

        public RssController(IRssService rssService)
        {
            _rssService = rssService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _rssService.GetAll());
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
            var rss = await _rssService.GetById(id.Value);
            await _rssService.Remove(rss);

            return RedirectToAction(nameof(Index));
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
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await RssExists(rss.Id)))
                    {
                        return NotFound();
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(rss);
        }

        private async Task<bool> RssExists(Guid id)
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
