using System;
using System.Collections.Generic;

namespace GoodNewsAggregator.DAL.Core.Entities
{
    public class Article : IBaseEntity
    {
        public Guid Id { get; set; }
        public string Source { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public int GoodFactor { get; set; }

        public Guid RssId { get; set; }
        public virtual Rss Rss { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
