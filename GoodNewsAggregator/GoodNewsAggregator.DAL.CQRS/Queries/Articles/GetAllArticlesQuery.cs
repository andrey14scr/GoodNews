using System.Collections.Generic;
using GoodNewsAggregator.Core.DTO;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Queries.Articles
{
    public class GetAllArticlesQuery : IRequest<IEnumerable<ArticleDto>>
    {
        
    }
}