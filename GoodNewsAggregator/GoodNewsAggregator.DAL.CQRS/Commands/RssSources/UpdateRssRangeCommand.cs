using System.Collections.Generic;
using GoodNewsAggregator.Core.DTO;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Commands.RssSources
{
    public class UpdateRssRangeCommand : IRequest<int>
    {
        public IEnumerable<RssDto> RssDtos { get; set; }
    }
}