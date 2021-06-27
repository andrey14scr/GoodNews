using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Queries.Users
{
    public class CheckPasswordQuery : IRequest<bool>
    {
        public string Password { get; set; }
        public string Email { get; set; }

        public CheckPasswordQuery(string password, string email)
        {
            Password = password;
            Email = email;
        }
    }
}