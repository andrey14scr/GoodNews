using System;
using System.Collections.Generic;
using System.Text;

namespace GoodNewsAggregator.DAL.Core
{
    public interface IBaseEntity
    {
        Guid Id { get; set; }
    }
}
