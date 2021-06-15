using System.Collections.Generic;
using GoodNewsAggregator.Core.DTO;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Commands.Articles
{
    public class AddArticlesRangeCommand : IRequest<int>
    {
        public IEnumerable<ArticleDto> ArticleDtos { get; set; }
    }
}