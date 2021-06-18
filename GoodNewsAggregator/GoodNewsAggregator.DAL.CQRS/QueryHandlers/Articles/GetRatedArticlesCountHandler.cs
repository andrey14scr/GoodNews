using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.CQRS.Queries.Articles;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GoodNewsAggregator.DAL.CQRS.QueryHandlers.Articles
{
    public class GetRatedArticlesCountHandler : IRequestHandler<GetRatedArticlesCountQuery, int>
    {
        private readonly GoodNewsAggregatorContext _dbContext;

        public GetRatedArticlesCountHandler(GoodNewsAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(GetRatedArticlesCountQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Articles.Where(a => a.GoodFactor.HasValue).CountAsync();
        }
    }
}