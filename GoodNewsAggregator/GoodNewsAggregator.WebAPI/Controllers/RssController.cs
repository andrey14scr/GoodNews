using Microsoft.AspNetCore.Mvc;

using System;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces;

namespace GoodNewsAggregator.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RssController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly IRssService _rssService;

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
            var commentDto = await _rssService.GetById(id);

            if (commentDto == null)
                return NotFound();

            return Ok(commentDto);
        }

        /// <summary>
        /// Get a collection of rss source
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var rssDtos = await _rssService.GetAll();

            if (rssDtos == null)
                return NotFound();

            return Ok(rssDtos);
        }

        /// <summary>
        /// Create a new rss source
        /// </summary>
        /// <param name="rssDto">RssDto that represents an information about rss source</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RssDto rssDto)
        {
            if (rssDto == null)
                return NotFound();

            await _rssService.Add(rssDto);
            return Ok();
        }

        /// <summary>
        /// Delete a rss source
        /// </summary>
        /// <param name="id">Id of rss source you want to delete</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var rssDto = await _rssService.GetById(id);

            if (rssDto == null)
                return NotFound();

            await _rssService.Remove(rssDto);

            return Ok();
        }

        /// <summary>
        /// Update a rss source
        /// </summary>
        /// <param name="rssDto">RssDto that represents an information about rss source</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] RssDto rssDto)
        {
            if (rssDto == null)
                return NotFound();

            await _rssService.Update(rssDto);
            return Ok();
        }
    }
}
