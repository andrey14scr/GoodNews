using System;

namespace GoodNewsAggregator.Models
{
    public class ArticleViewModel
    {
        public Guid Id { get; set; }
        public string Source { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
