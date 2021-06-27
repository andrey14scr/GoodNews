using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces.Enums;
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
            var result = _dbContext.Articles.Where(a => a.GoodFactor.HasValue != request.HasNulls);

            switch (request.SortByOption)
            {
                case SortByOption.DateTime:
                    result = result.OrderByDescending(a => a.Date);
                    break;
                case SortByOption.GoodFactor:
                    result = result.OrderByDescending(a => a.GoodFactor);
                    break;
            }

            return _mapper.Map<List<ArticleDto>>(await result
                .Skip(request.Skip)
                .Take(request.Take)
                .AsNoTracking()
                .ToListAsync());
        }
    }
}