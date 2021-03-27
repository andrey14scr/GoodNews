using System;
using System.Collections.Generic;
using System.Text;

namespace GoodNewsAggregator.Core.DTO
{
    public class ArticleDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public float GoodFactor { get; set; }

        public short SourceId { get; set; }
    }
}
