using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.CQRS.Queries.Articles;
using GoodNewsAggregator.DAL.CQRS.Queries.Comments;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GoodNewsAggregator.DAL.CQRS.QueryHandlers.Comments
{
    public class GetCommentsByArticleIdHandler : IRequestHandler<GetCommentsByArticleIdQuery, IEnumerable<CommentDto>>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public GetCommentsByArticleIdHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CommentDto>> Handle(GetCommentsByArticleIdQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<CommentDto>>(await _dbContext.Comments.Where(c => c.ArticleId.Equals(request.ArticleId)).ToListAsync());
        }
    }
}