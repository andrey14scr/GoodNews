﻿using System;
using System.Collections.Generic;

namespace GoodNewsAggregator.DAL.Core
{
    public class Article : IBaseEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public float GoodFactor { get; set; }

        public Guid SourceId { get; set; }
        public virtual Source Source { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
