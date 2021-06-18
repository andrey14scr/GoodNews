using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.DAL.Core.Entities;

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

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var commentDto = await _rssService.GetById(id);

            if (commentDto == null)
                return NotFound();

            return Ok(commentDto);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var rssDtos = await _rssService.GetAll();

            if (rssDtos == null)
                return NotFound();

            return Ok(rssDtos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RssDto rssDto)
        {
            if (rssDto == null)
                return NotFound();

            await _rssService.Add(rssDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var rssDto = await _rssService.GetById(id);

            if (rssDto == null)
                return NotFound();

            await _rssService.Remove(rssDto);

            return Ok();
        }

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
