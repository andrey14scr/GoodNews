using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GoodNewsAggregator.DAL.CQRS.Queries.Users
{
    public class LoginUserQuery : IRequest<SignInResult>
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