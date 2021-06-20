using System.Threading;
using System.Threading.Tasks;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.CQRS.Queries.RefreshTokens;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GoodNewsAggregator.DAL.CQRS.QueryHandlers.RefreshTokens
{
    public class GetRefreshTokenByUserNameHandler : IRequestHandler<GetRefreshTokenByUserNameQuery, RefreshToken>
    {
        private readonly GoodNewsAggregatorContext _dbContext;

        public GetRefreshTokenByUserNameHandler(GoodNewsAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RefreshToken> Handle(GetRefreshTokenByUserNameQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.RefreshTokens.AsNoTracking().FirstOrDefaultAsync(rt => rt.UserName == request.UserName);
        }
    }
}