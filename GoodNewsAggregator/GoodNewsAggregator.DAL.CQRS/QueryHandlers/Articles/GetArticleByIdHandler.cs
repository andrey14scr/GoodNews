using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.CQRS.Queries;
using GoodNewsAggregator.DAL.CQRS.Queries.Articles;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GoodNewsAggregator.DAL.CQRS.QueryHandlers.Articles
{
    public class GetArticleByIdHandler : IRequestHandler<GetArticleByIdQuery, ArticleDto>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public GetArticleByIdHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ArticleDto> Handle(GetArticleByIdQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<ArticleDto>(await _dbContext.Articles.AsNoTracking().FirstOrDefaultAsync(a => a.Id.Equals(request.Id)));
        }
    }
}