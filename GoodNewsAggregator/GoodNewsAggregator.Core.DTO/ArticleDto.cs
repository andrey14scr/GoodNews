﻿using System;

namespace GoodNewsAggregator.Core.DTO
{
    public class ArticleDto
    {
        public Guid Id { get; set; }
        public string Source { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public float? GoodFactor { get; set; }

        public Guid RssId { get; set; }
    }
}
