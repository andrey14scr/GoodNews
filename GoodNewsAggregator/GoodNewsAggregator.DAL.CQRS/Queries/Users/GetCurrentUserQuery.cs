using System.Security.Claims;
using GoodNewsAggregator.Core.DTO;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Queries.Users
{
    public class GetCurrentUserQuery : IRequest<UserDto>
    {
        public ClaimsPrincipal Claims { get; set; }

        public GetCurrentUserQuery(ClaimsPrincipal claims)
        {
            Claims = claims;
        }
    }
}