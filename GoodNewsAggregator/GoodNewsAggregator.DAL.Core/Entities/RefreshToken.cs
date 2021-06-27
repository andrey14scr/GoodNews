using System;

namespace GoodNewsAggregator.DAL.Core.Entities
{
    public class RefreshToken : IBaseEntity
    {
        public Guid Id { get; set; }
        public DateTime ExpireAt { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}