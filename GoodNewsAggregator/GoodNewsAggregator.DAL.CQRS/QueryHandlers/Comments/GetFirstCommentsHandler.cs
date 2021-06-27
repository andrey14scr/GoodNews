using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.CQRS.Queries.Comments;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GoodNewsAggregator.DAL.CQRS.QueryHandlers.Comments
{
    public class GetFirstCommentsHandler : IRequestHandler<GetFirstCommentsQuery, IEnumerable<CommentDto>>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public GetFirstCommentsHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CommentDto>> Handle(GetFirstCommentsQuery request, CancellationToken cancellationToken)
        {
            var comments = await _dbContext.Comments
                .Where(c => c.ArticleId == request.ArticleId)
                .Skip(request.Skip)
                .Take(request.Take)
                .ToListAsync();

            return _mapper.Map<IEnumerable<CommentDto>>(comments);
        }
    }
}