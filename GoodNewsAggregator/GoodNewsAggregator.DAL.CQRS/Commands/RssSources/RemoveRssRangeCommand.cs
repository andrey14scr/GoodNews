using System.Collections.Generic;
using GoodNewsAggregator.Core.DTO;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Commands.RssSources
{
    public class RemoveRssRangeCommand : IRequest<int>
    {
        public IEnumerable<RssDto> RssDtos { get; set; }

        public RemoveRssRangeCommand(IEnumerable<RssDto> rssDto)
        {
            RssDtos = rssDto;
        }
    }
}