using GoodNewsAggregator.DAL.Core.Entities;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Queries.RefreshTokens
{
    public class GetRefreshTokenByUserNameQuery : IRequest<RefreshToken>
    {
        public string UserName { get; set; }

        public GetRefreshTokenByUserNameQuery(string userName)
        {
            UserName = userName;
        }
    }
}