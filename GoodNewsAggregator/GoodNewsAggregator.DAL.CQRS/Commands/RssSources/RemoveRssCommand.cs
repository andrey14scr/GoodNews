using GoodNewsAggregator.Core.DTO;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Commands.RssSources
{
    public class RemoveRssCommand : IRequest<int>
    {
        public RssDto RssDto { get; set; }

        public RemoveRssCommand(RssDto rssDto)
        {
            RssDto = rssDto;
        }
    }
}