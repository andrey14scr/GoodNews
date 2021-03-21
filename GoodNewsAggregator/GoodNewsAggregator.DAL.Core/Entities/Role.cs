using System;
using System.Collections.Generic;

namespace GoodNewsAggregator.DAL.Core
{
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
