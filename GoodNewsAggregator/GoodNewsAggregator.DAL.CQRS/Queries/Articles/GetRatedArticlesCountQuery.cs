using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Queries.Articles
{
    public class GetRatedArticlesCountQuery : IRequest<int>
    {
        
    }
}