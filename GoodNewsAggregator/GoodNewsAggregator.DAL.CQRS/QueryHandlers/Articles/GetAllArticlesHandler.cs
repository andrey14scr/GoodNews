using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.CQRS.Queries.Articles;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GoodNewsAggregator.DAL.CQRS.QueryHandlers.Articles
{
    public class GetAllArticlesHandler : IRequestHandler<GetAllArticlesQuery, IEnumerable<ArticleDto>>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public GetAllArticlesHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ArticleDto>> Handle(GetAllArticlesQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<ArticleDto>>(await _dbContext.Articles.AsNoTracking().ToListAsync());
        }
    }
}