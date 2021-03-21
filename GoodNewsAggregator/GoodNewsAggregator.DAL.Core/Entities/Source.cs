using System;
using System.Collections.Generic;

namespace GoodNewsAggregator.DAL.Core
{
    public class Source : IBaseEntity
    {
        public Guid Id { get; set; }
        public string Url { get; set; }

        public virtual ICollection<Article> Articles { get; set; }
    }
}
