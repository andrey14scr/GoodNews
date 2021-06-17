using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodNewsAggregator.DAL.Core.Entities;

namespace GoodNewsAggregator.Models
{
    public class NewsListModel
    {
        public IEnumerable<Article> list { get; set; }
    }
}
