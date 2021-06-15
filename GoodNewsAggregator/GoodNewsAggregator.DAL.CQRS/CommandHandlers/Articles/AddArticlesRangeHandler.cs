using System.Collections.Generic;
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

namespace GoodNewsAggregator.DAL.CQRS.CommandHandlers.Articles
{
    public class AddArticlesRangeHandler : IRequestHandler<AddArticlesRangeCommand, int>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public AddArticlesRangeHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(AddArticlesRangeCommand request, CancellationToken cancellationToken)
        {
            var articles = _mapper.Map<IEnumerable<Article>>(request.ArticleDtos);
            await _dbContext.Articles.AddRangeAsync(articles);
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}