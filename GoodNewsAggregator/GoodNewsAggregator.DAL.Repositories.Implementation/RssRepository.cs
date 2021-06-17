using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Core.Entities;

namespace GoodNewsAggregator.DAL.Repositories.Implementation
{
    public class RssRepository : Repository<Rss>
    {
        public RssRepository(GoodNewsAggregatorContext context) : base(context)
        { }
    }
}
