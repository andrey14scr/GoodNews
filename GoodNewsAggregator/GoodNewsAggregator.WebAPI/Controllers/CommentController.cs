using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.Core.Services.Interfaces.Constants;
using Microsoft.AspNetCore.Authorization;
using Serilog;

namespace GoodNewsAggregator.WebAPI.Controllers
{
    /// <summary>
    /// Controller for work with comments from db
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        /// <summary>
        /// CommentController constructor
        /// </summary>
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
            try
            {
                var commentDto = await _commentService.GetById(id);

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
        /// Get a collection of comments
        /// </summary>
        /// <param name="articleId">Id of an article, comments of which you want to get</param>
        /// <param name="skip">How many comments you don't need to take from the beginning</param>
        /// <param name="take">How many comments you want to take</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get(Guid? articleId, int? skip, int? take)
        {
            if (skip.HasValue ^ take.HasValue)
                return BadRequest("Both parameters \"skip\" and \"take\" must be null or have values");

            var commentDtos = new List<CommentDto>();

            try
            {
                if (articleId.HasValue)
                {
                    if (skip.HasValue)
                        commentDtos = (await _commentService.GetFirst(articleId.Value, skip.Value, take.Value))
                            .ToList();
                    else
                        commentDtos = (await _commentService.GetByArticleId(articleId.Value)).ToList();
                }
                else
                    commentDtos = (await _commentService.GetAll()).ToList();

                return Ok(commentDtos);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Create a new comment
        /// </summary>
        /// <param name="commentDto">CommentDto that represents an information about comment</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = RoleNames.ADMIN)]
        public async Task<IActionResult> Create([FromBody] CommentDto commentDto)
        {
            if (commentDto == null)
                return NotFound();

            try
            {
                await _commentService.Add(commentDto);
                return Ok();
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Delete a comment
        /// </summary>
        /// <param name="id">Id of comment you want to delete</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = RoleNames.ADMIN)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var commentDto = await _commentService.GetById(id);

                if (commentDto == null)
                    return NotFound();

                await _commentService.Remove(commentDto);

                return Ok();
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Update a comment
        /// </summary>
        /// <param name="commentDto">CommentDto that represents an information about comment</param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Roles = RoleNames.ADMIN)]
        public async Task<IActionResult> Update([FromBody] CommentDto commentDto)
        {
            if (commentDto == null)
                return NotFound();

            try
            {
                await _commentService.Update(commentDto);
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
