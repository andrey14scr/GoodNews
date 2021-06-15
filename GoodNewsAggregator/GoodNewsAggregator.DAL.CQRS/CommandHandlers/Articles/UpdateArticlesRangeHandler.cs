using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.CQRS.Commands.Articles;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GoodNewsAggregator.DAL.CQRS.CommandHandlers.Articles
{
    public class UpdateArticlesRangeHandler : IRequestHandler<UpdateArticlesRangeCommand, int>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public UpdateArticlesRangeHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(UpdateArticlesRangeCommand request, CancellationToken cancellationToken)
        {
            var articles = _mapper.Map<IEnumerable<Article>>(request.ArticleDtos);
            _dbContext.Articles.UpdateRange(articles);
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}