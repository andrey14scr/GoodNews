using System;

namespace GoodNewsAggregator.Core.DTO
{
    public class RefreshTokenDto
    {
        public Guid Id { get; set; }
        public DateTime ExpireAt { get; set; }
        public Guid UserId { get; set; }
    }
}