using System;
using System.Collections.Generic;
using System.Text;

namespace GoodNewsAgregator.Data
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public Guid ArticleId { get; set; }   
    }
}
