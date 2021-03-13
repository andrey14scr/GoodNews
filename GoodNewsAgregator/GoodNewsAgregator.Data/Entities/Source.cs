using System;
using System.Collections.Generic;

namespace GoodNewsAggregator.DAL.Core
{
    public class Source
    {
        public short Id { get; set; }
        public string Url { get; set; }

        public virtual ICollection<Article> Articles { get; set; }
    }
}
