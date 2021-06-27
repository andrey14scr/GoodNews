using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Core.Entities;

namespace GoodNewsAggregator.DAL.Repositories.Implementation
{
    public class CommentsRepository : Repository<Comment>
    {
        public CommentsRepository(GoodNewsAggregatorContext context) : base(context)
        { }
    }
}
