﻿using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.CQRS.Commands.Articles;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.CommandHandlers.Articles
{
    public class UpdateArticleHandler : IRequestHandler<UpdateArticleCommand, int>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public UpdateArticleHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(UpdateArticleCommand request, CancellationToken cancellationToken)
        {
            _dbContext.Articles.Update(_mapper.Map<Article>(request));
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}