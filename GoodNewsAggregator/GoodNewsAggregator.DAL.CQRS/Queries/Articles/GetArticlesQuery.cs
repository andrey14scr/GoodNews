using System.Linq;
using GoodNewsAggregator.DAL.Core.Entities;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Queries.Articles
{
    public class GetArticlesQuery : IRequest<IQueryable<Article>>
    {
    }
}