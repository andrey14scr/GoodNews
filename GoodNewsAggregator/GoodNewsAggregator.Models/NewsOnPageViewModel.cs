using System.Collections.Generic;

namespace GoodNewsAggregator.Models
{
    public class NewsOnPageViewModel
    {
        public IEnumerable<ArticleInfoViewModel> ArticleInfos { get; set; }
        public PageInfo PageInfo { get; set; }

    }
}
