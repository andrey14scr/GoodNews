using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.Core.Services.Interfaces.Constants;
using GoodNewsAggregator.Core.Services.Interfaces.Enums;
using Microsoft.AspNetCore.Authorization;
using Serilog;

namespace GoodNewsAggregator.WebAPI.Controllers
{
    /// <summary>
    /// Controller for work with articles from db
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;

        /// <summary>
        /// ArticleController constructor
        /// </summary>
        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        /// <summary>
        /// Get a single article
        /// </summary>
        /// <param name="id">Id of some article</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var articleDto = await _articleService.GetById(id);

                if (articleDto == null)
                    return NotFound();

                return Ok(articleDto);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Get a collection of articles
        /// </summary>
        /// <param name="skip">How many articles you don't need to take from the beginning</param>
        /// <param name="take">How many articles you want to take</param>
        /// <param name="hasNulls">Should articles in result collection have null good factors</param>
        /// <param name="sortBy">Which sort we should use while looking for news
        /// <para>0 - sort by DateTime,</para>
        /// <para>1 - sort by GoodFactor</para>
        /// </param>
        [HttpGet]
        public async Task<IActionResult> Get(int? skip, int? take, bool? hasNulls, SortByOption? sortBy)
        {
            if (skip.HasValue && !take.HasValue)
                return BadRequest("Parameter \"skip\" must be with \"take\" parameter");

            if (!hasNulls.HasValue)
                hasNulls = false;

            if (!skip.HasValue)
                skip = 0;
            
            if (!sortBy.HasValue)
                sortBy = SortByOption.DateTime;
            else if (!Enum.IsDefined(sortBy.Value))
                return BadRequest($"Parameter \"sortBy\" can't take value = {sortBy.Value}. " +
                                  "Available values: \n" +
                                  "0 - sort by DateTime,\n" +
                                  "1 - sort by GoodFactor");

            var articleDtos = new List<ArticleDto>();

            try
            {
                if (take.HasValue)
                    articleDtos = (await _articleService.GetFirst(skip.Value, take.Value, hasNulls.Value, sortBy.Value))
                        .ToList();
                else
                    articleDtos = (await _articleService.GetAll()).ToList();

                return Ok(articleDtos);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Create a new article
        /// </summary>
        /// <param name="articleDto">ArticleDto that represents an information about article</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = RoleNames.ADMIN)]
        public async Task<IActionResult> Create([FromBody] ArticleDto articleDto)
        {
            if (articleDto == null)
                return NotFound();

            try
            {
                await _articleService.Add(articleDto);
                return Ok();
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Delete an article
        /// </summary>
        /// <param name="id">Id of article you want to delete</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = RoleNames.ADMIN)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var articleDto = await _articleService.GetById(id);

                if (articleDto == null)
                    return NotFound();

                await _articleService.Remove(articleDto);

                return Ok();
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Update an article
        /// </summary>
        /// <param name="articleDto">ArticleDto that represents an information about article</param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = RoleNames.ADMIN)]
        public async Task<IActionResult> Update([FromBody] ArticleDto articleDto)
        {
            if (articleDto == null)
                return NotFound();

            try
            {
                await _articleService.Update(articleDto);
                return Ok();
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return StatusCode(500, e.Message);
            }
        }
    }
}
