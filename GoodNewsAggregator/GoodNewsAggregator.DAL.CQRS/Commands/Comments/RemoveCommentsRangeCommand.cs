using System;
using System.Collections.Generic;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Commands.Comments
{
    public class RemoveCommentsRangeCommand : IRequest<int>
    {
        public IEnumerable<Guid> Ids { get; set; }

        public RemoveCommentsRangeCommand(IEnumerable<Guid> ids)
        {
            Ids = ids;
        }
    }
}