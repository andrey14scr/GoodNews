using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.DAL.Core;

using Microsoft.AspNetCore.Mvc.Rendering;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsAggregator.Models
{
    public class ArticleWithRssModel
    {
        public Guid Id { get; set; }
        public string Source { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public float GoodFactor { get; set; }

        public Guid RssId { get; set; }

        public SelectList RssList { get; set; }
    }
}
