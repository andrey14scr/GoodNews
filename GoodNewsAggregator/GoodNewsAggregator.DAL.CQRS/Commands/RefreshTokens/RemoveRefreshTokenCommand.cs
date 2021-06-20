using GoodNewsAggregator.DAL.Core.Entities;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Commands.RefreshTokens
{
    public class RemoveRefreshTokenCommand : IRequest<int>
    {
        public RefreshToken RefreshToken { get; set; }

        public RemoveRefreshTokenCommand(RefreshToken refreshToken)
        {
            RefreshToken = refreshToken;
        }
    }
}