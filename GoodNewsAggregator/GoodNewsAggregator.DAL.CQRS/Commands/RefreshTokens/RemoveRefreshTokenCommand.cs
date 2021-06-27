using GoodNewsAggregator.Core.DTO;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Commands.RefreshTokens
{
    public class RemoveRefreshTokenCommand : IRequest<int>
    {
        public RefreshTokenDto RefreshToken { get; set; }

        public RemoveRefreshTokenCommand(RefreshTokenDto refreshToken)
        {
            RefreshToken = refreshToken;
        }
    }
}