using Microsoft.AspNetCore.Http;
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
    public class CommentController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly ICommentService _commentService;

        public CommentController(IArticleService articleService, ICommentService commentService)
        {
            _articleService = articleService;
            _commentService = commentService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var commentDto = await _commentService.GetById(id);

            if (commentDto == null)
                return NotFound();

            return Ok(commentDto);
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid? articleId)
        {
            IEnumerable<CommentDto> commentDtos = new List<CommentDto>();
            if (articleId.HasValue)
                commentDtos = await _commentService.GetByArticleId(articleId.Value);
            else
                commentDtos = await _commentService.GetAll();

            if (!commentDtos.Any())
                return NotFound();

            return Ok(commentDtos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CommentDto commentDto)
        {
            if (commentDto == null)
                return NotFound();

            await _commentService.Add(commentDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var commentDto = await _commentService.GetById(id);

            if (commentDto == null)
                return NotFound();

            await _commentService.Remove(commentDto);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CommentDto commentDto)
        {
            if (commentDto == null)
                return NotFound();

            await _commentService.Update(commentDto);
            return Ok();
        }
    }
}
