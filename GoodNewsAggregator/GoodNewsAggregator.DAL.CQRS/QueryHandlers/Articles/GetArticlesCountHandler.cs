using System.Threading;
using System.Threading.Tasks;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.CQRS.Queries.Articles;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GoodNewsAggregator.DAL.CQRS.QueryHandlers.Articles
{
    public class GetArticlesCountHandler : IRequestHandler<GetArticlesCountQuery, int>
    {
        private readonly GoodNewsAggregatorContext _dbContext;

        public GetArticlesCountHandler(GoodNewsAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(GetArticlesCountQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Articles.AsNoTracking().CountAsync();
        }
    }
}