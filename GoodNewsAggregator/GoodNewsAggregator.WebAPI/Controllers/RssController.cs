using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.Core.Services.Interfaces.Constants;
using Microsoft.AspNetCore.Authorization;
using Serilog;

namespace GoodNewsAggregator.WebAPI.Controllers
{
    /// <summary>
    /// Controller for work with rss sources from db
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RssController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly IRssService _rssService;

        /// <summary>
        /// RssController constructor
        /// </summary>
        public RssController(IArticleService articleService, IRssService rssService)
        {
            _articleService = articleService;
            _rssService = rssService;
        }

        /// <summary>
        /// Get a single rss source
        /// </summary>
        /// <param name="id">Id of some rss source</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var commentDto = await _rssService.GetById(id);

                if (commentDto == null)
                    return NotFound();

                return Ok(commentDto);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Get a collection of rss source
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var rssDtos = await _rssService.GetAll();

                if (rssDtos == null)
                    return NotFound();

                return Ok(rssDtos);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Create a new rss source
        /// </summary>
        /// <param name="rssDto">RssDto that represents an information about rss source</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = RoleNames.ADMIN)]
        public async Task<IActionResult> Create([FromBody] RssDto rssDto)
        {
            if (rssDto == null)
                return NotFound();

            try
            {
                await _rssService.Add(rssDto);
                return Ok();
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Delete a rss source
        /// </summary>
        /// <param name="id">Id of rss source you want to delete</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = RoleNames.ADMIN)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var rssDto = await _rssService.GetById(id);

                if (rssDto == null)
                    return NotFound();

                await _rssService.Remove(rssDto);

                return Ok();
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Update a rss source
        /// </summary>
        /// <param name="rssDto">RssDto that represents an information about rss source</param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = RoleNames.ADMIN)]
        public async Task<IActionResult> Update([FromBody] RssDto rssDto)
        {
            if (rssDto == null)
                return NotFound();

            try
            {
                await _rssService.Update(rssDto);
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
