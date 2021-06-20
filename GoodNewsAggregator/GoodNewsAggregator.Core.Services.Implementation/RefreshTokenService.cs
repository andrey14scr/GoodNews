using System;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.CQRS.Commands.RefreshTokens;
using GoodNewsAggregator.DAL.CQRS.Queries.RefreshTokens;
using MediatR;

namespace GoodNewsAggregator.Core.Services.Implementation
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IMediator _mediator;

        public RefreshTokenService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task AddOrUpdate(Guid id, string userName, DateTime expireAt)
        {
            await _mediator.Send(new AddOrUpdateRefreshTokenCommand(id, userName, expireAt));
        }

        public async Task<RefreshToken> GetRefreshTokenByUserName(string userName)
        {
            return await _mediator.Send(new GetRefreshTokenByUserNameQuery(userName));
        }

        public async Task Remove(RefreshToken refreshToken)
        {
            await _mediator.Send(new RemoveRefreshTokenCommand(refreshToken));
        }
    }
}