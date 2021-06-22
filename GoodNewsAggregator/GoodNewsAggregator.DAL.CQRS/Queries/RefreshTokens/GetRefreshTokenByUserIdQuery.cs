using System;
using GoodNewsAggregator.Core.DTO;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Queries.RefreshTokens
{
    public class GetRefreshTokenByUserIdQuery : IRequest<RefreshTokenDto>
    {
        public Guid UserId { get; set; }

        public GetRefreshTokenByUserIdQuery(Guid userName)
        {
            UserId = userName;
        }
    }
}