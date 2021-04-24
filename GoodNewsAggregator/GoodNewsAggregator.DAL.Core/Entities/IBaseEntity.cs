using System;

namespace GoodNewsAggregator.DAL.Core.Entities
{
    public interface IBaseEntity
    {
        Guid Id { get; set; }
    }
}
