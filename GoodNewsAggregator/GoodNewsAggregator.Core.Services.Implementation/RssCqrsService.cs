using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.DAL.CQRS.Commands.RssSources;
using GoodNewsAggregator.DAL.CQRS.Queries.RssSources;
using MediatR;

namespace GoodNewsAggregator.Core.Services.Implementation
{
    public class RssCqrsService : IRssService
    {
        private readonly IMediator _mediator;

        public RssCqrsService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IEnumerable<RssDto>> GetAll()
        {
            return await _mediator.Send(new GetAllRssQuery());
        }

        public async Task<RssDto> GetById(Guid id)
        {
            return await _mediator.Send(new GetRssByIdQuery(id));
        }

        public async Task Add(RssDto rssDto)
        {
            await _mediator.Send(new AddRssCommand(rssDto));
        }

        public async Task AddRange(IEnumerable<RssDto> rssDtos)
        {
            await _mediator.Send(new AddRssRangeCommand(rssDtos));
        }

        public async Task Update(RssDto rssDto)
        {
            await _mediator.Send(new UpdateRssCommand(rssDto));
        }

        public async Task Remove(RssDto rssDto)
        {
            await _mediator.Send(new RemoveRssCommand(rssDto));
        }

        public async Task RemoveRange(IEnumerable<RssDto> rssDtos)
        {
            await _mediator.Send(new RemoveRssRangeCommand(rssDtos));
        }
    }
}