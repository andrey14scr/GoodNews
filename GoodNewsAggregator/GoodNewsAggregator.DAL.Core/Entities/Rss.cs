using System;
using System.Collections.Generic;

namespace GoodNewsAggregator.DAL.Core.Entities
{
    public class Rss : IBaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public virtual ICollection<Article> Articles { get; set; }
    }
}
