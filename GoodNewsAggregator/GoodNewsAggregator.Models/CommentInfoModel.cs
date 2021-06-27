using System;

namespace GoodNewsAggregator.Models
{
    public class CommentInfoModel
    {
        public string Text { get; set; }
        public Guid ArticleId { get; set; }
    }
}
