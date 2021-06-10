using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsAggregator.Core.DTO
{
    public class ArticleWithRssNameDto
    {
        public Guid Id { get; set; }
        public string Source { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public int GoodFactor { get; set; }

        public Guid RssId { get; set; }
        public string RssName { get; set; }
    }
}
