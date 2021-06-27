using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Core.Entities;

namespace GoodNewsAggregator.DAL.Repositories.Implementation
{
    public class ArticlesRepository : Repository<Article>
    {
        public ArticlesRepository(GoodNewsAggregatorContext context) : base(context)
        { }
    }
}
