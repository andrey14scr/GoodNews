using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.Models;
using GoodNewsAggregator.Core.Services.Interfaces;

namespace GoodNewsAggregator.Controllers
{
    public class ArticleController : Controller
    {
        private readonly GoodNewsAggregatorContext _context;
        private readonly IRssService _rssService;

        public ArticleController(GoodNewsAggregatorContext context, IRssService rssService)
        {
            _context = context;
            _rssService = rssService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Articles.ToListAsync());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            return await FindArticle(id);
        }

        public async Task<IActionResult> CreateAsync()
        {
            var a = new Article();
            var c = await _rssService.GetAll();
            var b = new SelectList(await _rssService.GetAll(), "Id", "Name");

            var model = new ArticleWithRssModel()
            {
                //Article = new Article(),
                RssList = new SelectList(await _rssService.GetAll(), "Id", "Name")
            };

            return View(model);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            return await FindArticle(id);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            return await FindArticle(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Article article)
        {
            if (ModelState.IsValid)
            {
                article.Id = Guid.NewGuid();
                _context.Add(article);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Article article)
        {
            if (id != article.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(article);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.Id))
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

            return View(article);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var article = await _context.Articles.FindAsync(id);
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(Guid id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }

        private async Task<IActionResult> FindArticle(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles.FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }
    }
}
