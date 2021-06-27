using GoodNewsAggregator.Core.DTO;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Queries.Users
{
    public class GetUserByUserNameQuery : IRequest<UserDto>
    {
        public string UserName { get; set; }

        public GetUserByUserNameQuery(string userName)
        {
            UserName = userName;
        }
    }
}