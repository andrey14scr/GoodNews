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
    public class GetRssByIdHandler : IRequestHandler<GetRssByIdQuery, RssDto>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public GetRssByIdHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<RssDto> Handle(GetRssByIdQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<RssDto>(await _dbContext.Rss.AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id.Equals(request.Id)));
        }
    }
}