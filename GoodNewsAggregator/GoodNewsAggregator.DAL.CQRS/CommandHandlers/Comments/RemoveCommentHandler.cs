using System.Threading;
using System.Threading.Tasks;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.CQRS.Commands.Comments;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GoodNewsAggregator.DAL.CQRS.CommandHandlers.Comments
{
    public class RemoveCommentHandler : IRequestHandler<RemoveCommentCommand, int>
    {
        private readonly GoodNewsAggregatorContext _dbContext;

        public RemoveCommentHandler(GoodNewsAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(RemoveCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id.Equals(request.Id));
            _dbContext.Comments.Remove(comment);
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}