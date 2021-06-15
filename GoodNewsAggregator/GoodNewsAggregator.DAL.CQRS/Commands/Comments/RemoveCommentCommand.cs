using System;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Commands.Comments
{
    public class RemoveCommentCommand : IRequest<int>
    {
        public Guid Id { get; set; }

        public RemoveCommentCommand(Guid id)
        {
            Id = id;
        }
    }
}