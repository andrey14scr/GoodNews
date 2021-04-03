using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GoodNewsAggregator.DAL.Core;

namespace GoodNewsAggregator.Controllers
{
    public class RssController : Controller
    {
        private readonly GoodNewsAggregatorContext _context;

        public RssController(GoodNewsAggregatorContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Rss.ToListAsync());
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
        public async Task<IActionResult> Create(Rss rss)
        {
            if (ModelState.IsValid)
            {
                rss.Id = Guid.NewGuid();
                _context.Add(rss);
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }

            return View(rss);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Rss rss)
        {
            if (id != rss.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rss);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RssExists(rss.Id))
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
            var rss = await _context.Rss.FindAsync(id);
            _context.Rss.Remove(rss);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool RssExists(Guid id)
        {
            return _context.Rss.Any(e => e.Id == id);
        }

        private async Task<IActionResult> FindRss(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rss = await _context.Rss.FirstOrDefaultAsync(m => m.Id == id);
            if (rss == null)
            {
                return NotFound();
            }

            return View(rss);
        }
    }
}
