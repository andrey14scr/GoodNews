using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.CQRS.Commands.Articles;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.CommandHandlers.Articles
{
    public class RemoveArticlesRangeHandler : IRequestHandler<RemoveArticlesRangeCommand, int>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public RemoveArticlesRangeHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(RemoveArticlesRangeCommand request, CancellationToken cancellationToken)
        {
            _dbContext.Articles.RemoveRange(_mapper.Map<IEnumerable<Article>>(request.ArticleDtos));
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}