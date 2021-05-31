using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace GoodNewsAggregator.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly UserManager<User> _userManager;

        public CommentsController(ICommentService commentService, UserManager<User> userManager)
        {
            _commentService = commentService;
            _userManager = userManager;
        }

        public async Task<IActionResult> List(Guid articleId)
        {
            var comments = await _commentService.GetByArticleId(articleId);

            List<CommentViewModel> result = new List<CommentViewModel>();

            foreach (var item in comments)
            {
                result.Add(new CommentViewModel()
                {
                    Id = item.Id,
                    ArticleId = item.ArticleId,
                    Text = item.Text, 
                    Date = item.Date, 
                    UserName = _userManager.FindByIdAsync(item.UserId.ToString()).Result.UserName
                });
            }

            return View(new CommentsListModel
            {
                ArticleId = articleId,
                Comments = result
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CommentDto model)
        {
            /*
            var user = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimsIdentity.DefaultNameClaimType));
            var userEmail = user?.Value;
            var userId = (await _userService.GetUserByEmail(userEmail)).Id;

            var commentDto = new CommentDto()
            {
                Id = Guid.NewGuid(),
                NewsId = model.NewsId,
                Text = model.CommentText,
                Created = DateTime.Now,
                UserId = userId
            };
            await _commentService.AddComment(commentDto);
            */
            return Ok();
        }
    }
}
