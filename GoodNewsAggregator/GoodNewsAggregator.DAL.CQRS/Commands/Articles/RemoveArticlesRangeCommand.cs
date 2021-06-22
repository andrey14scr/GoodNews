using System.Collections.Generic;
using GoodNewsAggregator.Core.DTO;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Commands.Articles
{
    public class RemoveArticlesRangeCommand : IRequest<int>
    {
        public IEnumerable<ArticleDto> ArticleDto { get; set; }

        public RemoveArticlesRangeCommand(IEnumerable<ArticleDto> articleDto)
        {
            ArticleDto = articleDto;
        }
    }
}