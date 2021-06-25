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
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        /// <summary>
        /// Create a comment
        /// </summary>
        /// <param name="id">Id of some comment</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var commentDto = await _commentService.GetById(id);

            if (commentDto == null)
                return NotFound();

            return Ok(commentDto);
        }

        /// <summary>
        /// Get a collection of comments
        /// </summary>
        /// <param name="articleId">Id of an article, comments of which you want to get</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get(Guid? articleId)
        {
            IEnumerable<CommentDto> commentDtos;
            if (articleId.HasValue)
                commentDtos = await _commentService.GetByArticleId(articleId.Value);
            else
                commentDtos = await _commentService.GetAll();

            if (!commentDtos.Any())
                return NotFound();

            return Ok(commentDtos);
        }

        /// <summary>
        /// Create a new comment
        /// </summary>
        /// <param name="commentDto">CommentDto that represents an information about comment</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CommentDto commentDto)
        {
            if (commentDto == null)
                return NotFound();

            await _commentService.Add(commentDto);
            return Ok();
        }

        /// <summary>
        /// Delete a comment
        /// </summary>
        /// <param name="id">Id of comment you want to delete</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var commentDto = await _commentService.GetById(id);

            if (commentDto == null)
                return NotFound();

            await _commentService.Remove(commentDto);

            return Ok();
        }

        /// <summary>
        /// Update a comment
        /// </summary>
        /// <param name="commentDto">CommentDto that represents an information about comment</param>
        /// <returns></returns>
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
