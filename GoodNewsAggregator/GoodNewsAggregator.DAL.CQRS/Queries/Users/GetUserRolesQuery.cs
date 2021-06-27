using System.Security.Claims;
using GoodNewsAggregator.Core.DTO;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Queries.Users
{
    public class GetUserRolesQuery : IRequest<string>
    {
        public UserDto UserDto { get; set; }

        public GetUserRolesQuery(UserDto userDto)
        {
            UserDto = userDto;
        }
    }
}