using System;
using System.Collections.Generic;
using GoodNewsAggregator.Core.DTO;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Queries.Comments
{
    public class GetCommentsByArticleIdQuery : IRequest<IEnumerable<CommentDto>>
    {
        public Guid ArticleId { get; set; }

        public GetCommentsByArticleIdQuery(Guid articleId)
        {
            ArticleId = articleId;
        }
    }
}