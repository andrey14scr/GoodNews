﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsAggregator.Models
{
    public class ArticleInfoViewModel
    {
        public Guid Id { get; set; }
        public string Source { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public float? GoodFactor { get; set; }
        public string RssName { get; set; }
    }
}
