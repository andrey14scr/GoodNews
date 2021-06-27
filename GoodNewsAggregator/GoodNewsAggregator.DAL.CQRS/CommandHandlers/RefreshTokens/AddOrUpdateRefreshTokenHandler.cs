using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.CQRS.Commands.RefreshTokens;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GoodNewsAggregator.DAL.CQRS.CommandHandlers.RefreshTokens
{
    public class AddOrUpdateRefreshTokenHandler : IRequestHandler<AddOrUpdateRefreshTokenCommand, int>
    {
        private readonly GoodNewsAggregatorContext _dbContext;

        public AddOrUpdateRefreshTokenHandler(GoodNewsAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(AddOrUpdateRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = _dbContext.RefreshTokens.AsNoTracking()
                .FirstOrDefault(rt => rt.Id == request.Id);
            
            if (refreshToken == null)
            {
                refreshToken = new RefreshToken()
                {
                    Id = request.Id, 
                    UserId = request.UserId, 
                    ExpireAt = request.ExpireAt
                };

                await _dbContext.RefreshTokens.AddAsync(refreshToken);
            }
            else
            {
                refreshToken.UserId = request.UserId;
                refreshToken.ExpireAt = request.ExpireAt;

                _dbContext.RefreshTokens.Update(refreshToken);
            }

            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}