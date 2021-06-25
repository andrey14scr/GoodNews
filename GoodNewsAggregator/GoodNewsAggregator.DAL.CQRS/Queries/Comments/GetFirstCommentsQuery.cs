using System;
using System.Collections.Generic;
using GoodNewsAggregator.Core.DTO;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Queries.Comments
{
    public class GetFirstCommentsQuery : IRequest<IEnumerable<CommentDto>>
    {
        public Guid ArticleId { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }

        public GetFirstCommentsQuery(Guid articleId, int skip, int take)
        {
            ArticleId = articleId;
            Skip = skip;
            Take = take;
        }
    }
}