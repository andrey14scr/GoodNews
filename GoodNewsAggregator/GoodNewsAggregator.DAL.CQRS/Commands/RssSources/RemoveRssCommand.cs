using System;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Commands.RssSources
{
    public class RemoveRssCommand : IRequest<int>
    {
        public Guid Id { get; set; }

        public RemoveRssCommand(Guid id)
        {
            Id = id;
        }
    }
}