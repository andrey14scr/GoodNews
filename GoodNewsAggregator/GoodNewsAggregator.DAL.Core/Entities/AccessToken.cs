using System;

namespace GoodNewsAggregator.DAL.Core.Entities
{
    public class AccessToken : IBaseEntity
    {
        public Guid Id { get; set; }
        public string Value { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}