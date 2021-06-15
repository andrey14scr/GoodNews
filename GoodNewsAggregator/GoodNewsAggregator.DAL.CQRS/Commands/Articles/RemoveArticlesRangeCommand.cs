using System;
using System.Collections.Generic;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Commands.Articles
{
    public class RemoveArticlesRangeCommand : IRequest<int>
    {
        public IEnumerable<Guid> Ids { get; set; }

        public RemoveArticlesRangeCommand(IEnumerable<Guid> ids)
        {
            Ids = ids;
        }
    }
}