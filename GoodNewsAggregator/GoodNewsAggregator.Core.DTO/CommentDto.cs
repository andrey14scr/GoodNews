using System;

namespace GoodNewsAggregator.Core.DTO
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public Guid ArticleId { get; set; }
        public Guid UserId { get; set; }
    }
}
