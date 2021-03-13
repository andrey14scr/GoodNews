using GoodNewsAggregator.DAL.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsAggregator.Models
{
    public class NewsListModel
    {
        public IEnumerable<Article> list { get; set; }
    }
}
