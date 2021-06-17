﻿using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.CQRS.Commands.Comments;
using GoodNewsAggregator.DAL.CQRS.Commands.RssSources;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GoodNewsAggregator.DAL.CQRS.CommandHandlers.RssSources
{
    public class RemoveRssHandler : IRequestHandler<RemoveRssCommand, int>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public RemoveRssHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(RemoveRssCommand request, CancellationToken cancellationToken)
        {
            var rss = await _dbContext.Rss.FirstOrDefaultAsync(r => r.Id.Equals(request.Id));
            _dbContext.Rss.Remove(rss);
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}