using GoodNewsAgregator.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsAgregator.Models
{
    public class NewsListModel
    {
        public IEnumerator<Article> list { get; set; }
    }
}
