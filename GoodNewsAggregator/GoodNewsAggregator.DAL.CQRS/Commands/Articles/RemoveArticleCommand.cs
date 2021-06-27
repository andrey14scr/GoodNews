using GoodNewsAggregator.Core.DTO;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Commands.Articles
{
    public class RemoveArticleCommand : IRequest<int>
    {
        public ArticleDto ArticleDto { get; set; }

        public RemoveArticleCommand(ArticleDto articleDto)
        {
            ArticleDto = articleDto;
        }
    }
}