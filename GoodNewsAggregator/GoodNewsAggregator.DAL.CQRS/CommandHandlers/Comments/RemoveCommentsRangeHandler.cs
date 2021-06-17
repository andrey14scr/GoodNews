using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.CQRS.Commands.Comments;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.CommandHandlers.Comments
{
    public class RemoveCommentsRangeHandler : IRequestHandler<RemoveCommentsRangeCommand, int>
    {
        private readonly GoodNewsAggregatorContext _dbContext;

        public RemoveCommentsRangeHandler(GoodNewsAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(RemoveCommentsRangeCommand request, CancellationToken cancellationToken)
        {
            var comments = _dbContext.Comments.Where(c => request.Ids.Contains(c.Id));
            _dbContext.Comments.RemoveRange(comments);
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}