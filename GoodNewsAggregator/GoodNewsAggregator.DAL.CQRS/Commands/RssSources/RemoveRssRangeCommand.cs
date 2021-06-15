using System;
using System.Collections.Generic;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Commands.RssSources
{
    public class RemoveRssRangeCommand : IRequest<int>
    {
        public IEnumerable<Guid> Ids { get; set; }

        public RemoveRssRangeCommand(IEnumerable<Guid> ids)
        {
            Ids = ids;
        }
    }
}