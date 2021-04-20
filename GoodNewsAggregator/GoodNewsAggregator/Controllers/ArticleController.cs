using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.Core.DTO;
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
        private readonly IRssService _rssService;
        private readonly IArticleService _articleService;
        private readonly IMapper _mapper;

        public ArticleController(IArticleService articleService, IRssService rssService, IMapper mapper)
        {
            _articleService = articleService;
            _rssService = rssService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _articleService.GetAll());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            return await FindArticle(id);
        }

        public async Task<IActionResult> Create()
        {
            var model = new ArticleWithRssModel()
            {
                RssList = new SelectList(await _rssService.GetAll(), "Id", "Name")
            };

            return View(model);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleDto = await _articleService.GetById(id.Value);
            if (articleDto == null)
            {
                return NotFound();
            }

            var model = await GetArticleWithRssModel(articleDto);

            return View(model);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            return await FindArticle(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ArticleDto article)
        {
            if (ModelState.IsValid)
            {
                article.Id = Guid.NewGuid();
                await _articleService.Add(article);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ArticleWithRssModel articleWithRssModel)
        {
            if (id != articleWithRssModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _articleService.Update(GetArticleFromModel(articleWithRssModel));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(articleWithRssModel.Id))
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

            return View(articleWithRssModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _articleService.Remove(await _articleService.GetById(id));

            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(Guid id)
        {
            return _articleService.GetById(id) == null;
        }

        private async Task<IActionResult> FindArticle(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _articleService.GetById(id.Value);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        private async Task<ArticleWithRssModel> GetArticleWithRssModel(ArticleDto article)
        {
            var model = new ArticleWithRssModel()
            {
                Id = article.Id,
                Source = article.Source,
                Title = article.Title,
                Content = article.Content,
                Date = article.Date,
                GoodFactor = article.GoodFactor,
                RssId = article.RssId,
                RssList = new SelectList(await _rssService.GetAll(), "Id", "Name")
            };

            return model;
        }

        private ArticleDto GetArticleFromModel(ArticleWithRssModel articleWithRssModel)
        {
            var article = new ArticleDto()
            {
                Id = articleWithRssModel.Id,
                Source = articleWithRssModel.Source,
                Title = articleWithRssModel.Title,
                Content = articleWithRssModel.Content,
                Date = articleWithRssModel.Date,
                GoodFactor = articleWithRssModel.GoodFactor,
                RssId = articleWithRssModel.RssId,
            };

            return article;
        }
    }
}
