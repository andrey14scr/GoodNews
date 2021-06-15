using System;
using System.Collections.Generic;
using GoodNewsAggregator.Core.DTO;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Queries.Articles
{
    public class GetArticlesByRssIdQuery : IRequest<IEnumerable<ArticleDto>>
    {
        public Guid RssId { get; set; }

        public GetArticlesByRssIdQuery(Guid rssId)
        {
            RssId = rssId;
        }
    }
}