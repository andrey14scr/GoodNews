using GoodNewsAggregator.Core.DTO;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Queries.Users
{
    public class GetUserByUserNameQuery : IRequest<UserDto>
    {
        public string Name { get; set; }

        public GetUserByUserNameQuery(string name)
        {
            Name = name;
        }
    }
}