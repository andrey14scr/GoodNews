using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.CQRS.Commands.Articles;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GoodNewsAggregator.DAL.CQRS.CommandHandlers.Articles
{
    public class RemoveArticleHandler : IRequestHandler<RemoveArticleCommand, int>
    {
        private readonly GoodNewsAggregatorContext _dbContext;

        public RemoveArticleHandler(GoodNewsAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(RemoveArticleCommand request, CancellationToken cancellationToken)
        {
            var article = await _dbContext.Articles.FirstOrDefaultAsync(a => a.Id.Equals(request.Id));
            _dbContext.Articles.Remove(article);
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}