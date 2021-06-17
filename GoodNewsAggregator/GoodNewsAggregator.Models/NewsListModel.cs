using System.Collections.Generic;
using GoodNewsAggregator.DAL.Core.Entities;

namespace GoodNewsAggregator.Models
{
    public class NewsListModel
    {
        public IEnumerable<Article> list { get; set; }
    }
}
