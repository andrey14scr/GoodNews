using System.Collections.Generic;
using GoodNewsAggregator.Core.DTO;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Commands.Articles
{
    public class RemoveArticlesRangeCommand : IRequest<int>
    {
        public IEnumerable<ArticleDto> ArticleDtos { get; set; }

        public RemoveArticlesRangeCommand(IEnumerable<ArticleDto> articleDtos)
        {
            ArticleDtos = articleDtos;
        }
    }
}