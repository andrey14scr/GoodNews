using System;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Commands.Comments
{
    public class AddCommentCommand : IRequest<int>
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public Guid ArticleId { get; set; }
        public Guid UserId { get; set; }
    }
}