using System;
using System.Collections.Generic;
using System.Text;

namespace GoodNewsAggregator.Core.DTO
{
    public class RssDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Url { get; set; }
    }
}
