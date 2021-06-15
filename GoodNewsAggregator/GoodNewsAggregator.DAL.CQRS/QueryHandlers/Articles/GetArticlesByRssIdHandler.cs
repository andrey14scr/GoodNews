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
    public class GetArticlesByRssIdHandler : IRequestHandler<GetArticlesByRssIdQuery, IEnumerable<ArticleDto>>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public GetArticlesByRssIdHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ArticleDto>> Handle(GetArticlesByRssIdQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<ArticleDto>>(_dbContext.Articles.Where(a => a.RssId.Equals(request.RssId)));
        }
    }
}