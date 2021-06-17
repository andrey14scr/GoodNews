using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.CQRS.Commands.RssSources;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.CommandHandlers.RssSources
{
    public class RemoveRssRangeHandler : IRequestHandler<RemoveRssRangeCommand, int>
    {
        private readonly GoodNewsAggregatorContext _dbContext;

        public RemoveRssRangeHandler(GoodNewsAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(RemoveRssRangeCommand request, CancellationToken cancellationToken)
        {
            var rss = _dbContext.Rss.Where(r => request.Ids.Contains(r.Id));
            _dbContext.Rss.RemoveRange(rss);
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}