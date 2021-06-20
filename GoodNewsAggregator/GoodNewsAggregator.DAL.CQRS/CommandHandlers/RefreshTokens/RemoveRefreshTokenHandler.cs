using System.Threading;
using System.Threading.Tasks;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.CQRS.Commands.RefreshTokens;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.CommandHandlers.RefreshTokens
{
    public class RemoveRefreshTokenHandler : IRequestHandler<RemoveRefreshTokenCommand, int>
    {
        private readonly GoodNewsAggregatorContext _dbContext;

        public RemoveRefreshTokenHandler(GoodNewsAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(RemoveRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            _dbContext.RefreshTokens.Remove(request.RefreshToken);
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}