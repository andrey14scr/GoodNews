using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.CQRS.Queries.RssSources;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GoodNewsAggregator.DAL.CQRS.QueryHandlers.RssSources
{
    public class GetAllRssHandler : IRequestHandler<GetAllRssQuery, IEnumerable<RssDto>>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public GetAllRssHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RssDto>> Handle(GetAllRssQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<RssDto>>(await _dbContext.Rss.AsNoTracking().ToListAsync());
        }
    }
}