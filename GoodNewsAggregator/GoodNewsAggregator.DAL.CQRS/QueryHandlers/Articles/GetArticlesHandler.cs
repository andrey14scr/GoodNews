using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.CQRS.Queries.Articles;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GoodNewsAggregator.DAL.CQRS.QueryHandlers.Articles
{
    public class GetArticlesHandler : IRequestHandler<GetArticlesQuery, IQueryable<Article>>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public GetArticlesHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IQueryable<Article>> Handle(GetArticlesQuery request, CancellationToken cancellationToken)
        {
            return _dbContext.Articles.AsNoTracking().AsQueryable();
        }
    }
}