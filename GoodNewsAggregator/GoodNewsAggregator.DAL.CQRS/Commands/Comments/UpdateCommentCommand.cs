using System;
using GoodNewsAggregator.Core.DTO;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Commands.Comments
{
    public class UpdateCommentCommand : IRequest<int>
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public Guid ArticleId { get; set; }
        public Guid UserId { get; set; }

        public UpdateCommentCommand(CommentDto commentDto)
        {
            Id = commentDto.Id;
            Text = commentDto.Text;
            Date = commentDto.Date;
            ArticleId = commentDto.ArticleId;
            UserId = commentDto.UserId;
        }
    }
}