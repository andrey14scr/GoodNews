using System;

namespace GoodNewsAggregator.Models
{
    public class CommentViewModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public string UserName { get; set; }
    }
}
