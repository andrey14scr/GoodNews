using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsAggregator.Models
{
    public class CommentInfoModel
    {
        public string Text { get; set; }
        public Guid ArticleId { get; set; }
    }
}
