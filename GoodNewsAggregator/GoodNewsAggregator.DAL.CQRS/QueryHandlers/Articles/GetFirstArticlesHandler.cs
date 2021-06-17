using System.Collections.Generic;
using System.Linq;
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
    public class GetFirstArticlesHandler : IRequestHandler<GetFirstArticlesQuery, IEnumerable<ArticleDto>>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public GetFirstArticlesHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ArticleDto>> Handle(GetFirstArticlesQuery request, CancellationToken cancellationToken)
        {
            var result = await _dbContext.Articles
                .OrderByDescending(a => a.Date)
                .Skip(request.Skip)
                .Take(request.Take)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<List<ArticleDto>>(result);
        }
    }
}