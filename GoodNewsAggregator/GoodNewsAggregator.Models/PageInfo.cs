using System;
using GoodNewsAggregator.Core.Services.Interfaces.Enums;

namespace GoodNewsAggregator.Models
{
    public class PageInfo
    {
        public SortByOption SortByOption { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);

    }
}
