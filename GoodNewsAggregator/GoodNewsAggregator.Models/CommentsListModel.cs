using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsAggregator.Models
{
    public class CommentsListModel
    {
        public Guid ArticleId { get; set; }
        public IEnumerable<CommentViewModel> Comments { get; set; }

        public bool HasNext { get; set; }
    }
}
