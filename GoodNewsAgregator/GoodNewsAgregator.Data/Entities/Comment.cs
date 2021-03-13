using System;

namespace GoodNewsAggregator.DAL.Core
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public Guid ArticleId { get; set; }
        public virtual Article Article { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}
