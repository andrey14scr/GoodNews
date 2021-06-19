using GoodNewsAggregator.Core.DTO;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Queries.Users
{
    public class LoginUserQuery : IRequest<UserDto>
    {
        public string Password { get; set; }
        public string UserName { get; set; }

        public LoginUserQuery(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
    }
}