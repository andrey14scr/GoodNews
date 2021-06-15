using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.CQRS.Commands.Articles;
using GoodNewsAggregator.DAL.CQRS.Queries.Articles;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            var articles = _dbContext.Articles.Where(a => request.Ids.Contains(a.Id));
            _dbContext.Articles.RemoveRange(articles);
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}