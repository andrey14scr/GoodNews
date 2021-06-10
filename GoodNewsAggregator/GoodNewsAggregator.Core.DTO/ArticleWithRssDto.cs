using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GoodNewsAggregator.Core.DTO
{
    public class ArticleWithRssDto
    {
        public Guid Id { get; set; }
        public string Source { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public int GoodFactor { get; set; }

        public Guid RssId { get; set; }

        public SelectList RssList { get; set; }
    }
}
