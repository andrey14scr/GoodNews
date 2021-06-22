using System;
using GoodNewsAggregator.Core.DTO;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Commands.RefreshTokens
{
    public class AddOrUpdateRefreshTokenCommand : IRequest<int>
    {
        public Guid Id { get; set; }
        public DateTime ExpireAt { get; set; }
        public string Token { get; set; }
        public Guid UserId { get; set; }

        public AddOrUpdateRefreshTokenCommand(RefreshTokenDto refreshToken)
        {
            ExpireAt = refreshToken.ExpireAt;
            Id = refreshToken.Id;
            UserId = refreshToken.UserId;
        }
    }
}