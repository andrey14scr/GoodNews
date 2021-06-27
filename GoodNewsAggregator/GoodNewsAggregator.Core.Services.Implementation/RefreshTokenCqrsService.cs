using System;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.DAL.CQRS.Commands.RefreshTokens;
using GoodNewsAggregator.DAL.CQRS.Queries.RefreshTokens;
using MediatR;

namespace GoodNewsAggregator.Core.Services.Implementation
{
    public class RefreshTokenCqrsService : IRefreshTokenService
    {
        private readonly IMediator _mediator;

        public RefreshTokenCqrsService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task AddOrUpdate(RefreshTokenDto refreshToken)
        {
            await _mediator.Send(new AddOrUpdateRefreshTokenCommand(refreshToken));
        }

        public async Task<RefreshTokenDto> GetRefreshTokenByUserId(Guid userId)
        {
            return await _mediator.Send(new GetRefreshTokenByUserIdQuery(userId));
        }

        public async Task Remove(RefreshTokenDto refreshToken)
        {
            await _mediator.Send(new RemoveRefreshTokenCommand(refreshToken));
        }
    }
}