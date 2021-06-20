using System;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Commands.RefreshTokens
{
    public class AddOrUpdateRefreshTokenCommand : IRequest<int>
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public DateTime ExpireAt { get; set; }

        public AddOrUpdateRefreshTokenCommand(Guid id, string userName, DateTime expireAt)
        {
            Id = id;
            UserName = userName;
            ExpireAt = expireAt;
        }
    }
}