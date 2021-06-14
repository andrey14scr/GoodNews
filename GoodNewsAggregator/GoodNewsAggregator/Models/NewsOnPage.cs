using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DTO;

namespace GoodNewsAggregator.Models
{
    public class NewsOnPage
    {
        public IEnumerable<ArticleWithRssNameDto> Articles { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
