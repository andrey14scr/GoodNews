using System;
using GoodNewsAggregator.Core.Services.Interfaces.Enums;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Queries.Users
{
    public class FindSuchUserQuery : IRequest<EnumUserResults>
    {
        public Guid? Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }

        public FindSuchUserQuery(Guid? id, string email, string userName)
        {
            Id = id;
            Email = email;
            UserName = userName;
        }
    }
}