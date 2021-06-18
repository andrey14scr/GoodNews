using System.Collections.Generic;
using GoodNewsAggregator.Core.DTO;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Queries.Articles
{
    public class GetFirstArticlesQuery : IRequest<IEnumerable<ArticleDto>>
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool HasNulls { get; set; }

        public GetFirstArticlesQuery(int skip, int take, bool hasNulls)
        {
            Skip = skip;
            Take = take;
            HasNulls = hasNulls;
        }
    }
}