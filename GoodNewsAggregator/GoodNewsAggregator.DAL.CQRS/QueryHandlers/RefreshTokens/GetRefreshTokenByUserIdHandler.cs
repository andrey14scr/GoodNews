using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.CQRS.Queries.RefreshTokens;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GoodNewsAggregator.DAL.CQRS.QueryHandlers.RefreshTokens
{
    public class GetRefreshTokenByUserIdHandler : IRequestHandler<GetRefreshTokenByUserIdQuery, RefreshTokenDto>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public GetRefreshTokenByUserIdHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<RefreshTokenDto> Handle(GetRefreshTokenByUserIdQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<RefreshTokenDto>(await _dbContext.RefreshTokens.AsNoTracking()
                .FirstOrDefaultAsync(rt => rt.UserId == request.UserId)) ;
        }
    }
}