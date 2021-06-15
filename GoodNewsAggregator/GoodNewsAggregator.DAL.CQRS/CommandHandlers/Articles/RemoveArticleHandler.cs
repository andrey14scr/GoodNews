using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.CQRS.Commands.Articles;
using GoodNewsAggregator.DAL.CQRS.Queries.Articles;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace GoodNewsAggregator.DAL.CQRS.CommandHandlers.Articles
{
    public class RemoveArticleHandler : IRequestHandler<RemoveArticleCommand, int>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public RemoveArticleHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(RemoveArticleCommand request, CancellationToken cancellationToken)
        {
            var article = await _dbContext.Articles.FirstOrDefaultAsync(a => a.Id.Equals(request.Id));
            _dbContext.Articles.Remove(article);
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}