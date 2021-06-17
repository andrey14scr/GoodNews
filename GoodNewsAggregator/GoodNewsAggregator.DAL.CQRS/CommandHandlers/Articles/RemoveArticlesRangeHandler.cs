using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.CQRS.Commands.Articles;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.CommandHandlers.Articles
{
    public class RemoveArticlesRangeHandler : IRequestHandler<RemoveArticlesRangeCommand, int>
    {
        private readonly GoodNewsAggregatorContext _dbContext;

        public RemoveArticlesRangeHandler(GoodNewsAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(RemoveArticlesRangeCommand request, CancellationToken cancellationToken)
        {
            var articles = _dbContext.Articles.Where(a => request.Ids.Contains(a.Id));
            _dbContext.Articles.RemoveRange(articles);
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}