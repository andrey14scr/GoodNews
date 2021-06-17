using System;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Commands.Articles
{
    public class RemoveArticleCommand : IRequest<int>
    {
        public Guid Id { get; set; }

        public RemoveArticleCommand(Guid id)
        {
            Id = id;
        }
    }
}