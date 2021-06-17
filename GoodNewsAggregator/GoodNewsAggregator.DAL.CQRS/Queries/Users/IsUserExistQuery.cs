using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Queries.Users
{
    public class IsUserExistQuery : IRequest<bool>
    {
        public string Email { get; set; }

        public IsUserExistQuery(string email)
        {
            Email = email;
        }
    }
}