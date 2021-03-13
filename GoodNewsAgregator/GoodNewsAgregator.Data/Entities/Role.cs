using System;
using System.Collections.Generic;

namespace GoodNewsAggregator.DAL.Core
{
    public class Role
    {
        public byte Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
