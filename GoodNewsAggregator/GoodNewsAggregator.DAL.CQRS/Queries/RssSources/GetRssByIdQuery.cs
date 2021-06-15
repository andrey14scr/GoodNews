using System;
using GoodNewsAggregator.Core.DTO;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Queries.RssSources
{
    public class GetRssByIdQuery : IRequest<RssDto>
    {
        public Guid Id { get; set; }

        public GetRssByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}