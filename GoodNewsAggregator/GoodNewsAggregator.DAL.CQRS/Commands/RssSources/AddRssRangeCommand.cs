using System.Collections.Generic;
using GoodNewsAggregator.Core.DTO;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Commands.RssSources
{
    public class AddRssRangeCommand : IRequest<int>
    {
        public IEnumerable<RssDto> RssDtos { get; set; }

        public AddRssRangeCommand(IEnumerable<RssDto> rssDtos)
        {
            RssDtos = rssDtos;
        }
    }
}