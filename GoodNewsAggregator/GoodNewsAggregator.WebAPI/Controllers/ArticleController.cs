﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces;

namespace GoodNewsAggregator.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var articleDto = await _articleService.GetById(id);

            if (articleDto == null)
                return NotFound();

            return Ok(articleDto);
        }

        [HttpGet]
        public async Task<IActionResult> Get(int skip, int take, bool hasNulls)
        {
            var articleDtos = await _articleService.GetFirst(skip, take, hasNulls);

            if (articleDtos == null)
                return NotFound();

            return Ok(articleDtos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ArticleDto articleDto)
        {
            if (articleDto == null)
                return NotFound();

            await _articleService.Add(articleDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var articleDto = await _articleService.GetById(id);

            if (articleDto == null)
                return NotFound();

            await _articleService.Remove(articleDto);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ArticleDto articleDto)
        {
            if (articleDto == null)
                return NotFound();

            await _articleService.Update(articleDto);
            return Ok();
        }
    }
}
