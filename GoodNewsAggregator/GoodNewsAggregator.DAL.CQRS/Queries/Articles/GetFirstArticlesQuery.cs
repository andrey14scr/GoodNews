using System.Collections.Generic;
using System.Linq;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.DAL.Core.Entities;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Queries.Articles
{
    public class GetFirstArticlesQuery : IRequest<IEnumerable<ArticleDto>>
    {
        public int Skip { get; set; }
        public int Take { get; set; }

        public GetFirstArticlesQuery(int skip, int take)
        {
            Skip = skip;
            Take = take;
        }
    }
}