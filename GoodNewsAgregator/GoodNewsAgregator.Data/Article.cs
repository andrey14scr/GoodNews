using System;

namespace GoodNewsAgregator.Data
{
    public class Article
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public short SourceId { get; set; }
        public DateTime Date { get; set; }
        public float GoodFactor { get; set; }
    }
}
