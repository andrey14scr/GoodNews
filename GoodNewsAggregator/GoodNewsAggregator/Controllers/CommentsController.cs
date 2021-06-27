using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodNewsAggregator.Constants;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace GoodNewsAggregator.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public CommentsController(ICommentService commentService, UserManager<User> userManager, IConfiguration configuration)
        {
            _commentService = commentService;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<IActionResult> List(Guid articleId, int next, bool add = true)
        {
            var commentsSize = 0;
            if (!Int32.TryParse(_configuration["Constants:CommentsSize"], out commentsSize))
            {
                Log.Error("Constants:CommentsSize field is not valid");
                commentsSize = 5;
            }

            var comments = (await _commentService.GetByArticleId(articleId))
                .OrderByDescending(c => c.Date)
                .ToList();

            var amount = comments.Count();

            var list = comments.Skip(add ? next * commentsSize : 0)
                .Take(add ? commentsSize : (next + 1) * commentsSize)
                .ToList();

            var result = new List<CommentViewModel>();

            foreach (var item in list)
            {
                result.Add(new CommentViewModel()
                {
                    Id = item.Id,
                    Text = item.Text, 
                    Date = item.Date, 
                    UserName = _userManager.FindByIdAsync(item.UserId.ToString()).Result.UserName
                });
            }

            return View(new CommentsListModel
            {
                ArticleId = articleId,
                Comments = result,
                HasNext = amount - (next + 1) * commentsSize > 0
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CommentInfoModel model)
        {
            var id = _userManager.GetUserId(User);

            var commentDto = new CommentDto()
            {
                Id = Guid.NewGuid(), 
                ArticleId = model.ArticleId, 
                Date = DateTime.Now, 
                Text = model.Text, 
                UserId = Guid.Parse(id)
            };
            await _commentService.Add(commentDto);
            
            return Ok();
        }
    }
}
