using System.Collections.Generic;

namespace GoodNewsAggregator.Models
{
    public class NewsOnPage
    {
        public IEnumerable<ArticleInfoViewModel> ArticleInfos { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
