using System;
using GoodNewsAggregator.Core.DTO;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Queries
{
    public class GetArticleByIdQuery : IRequest<ArticleDto>
    {
        public Guid Id { get; set; }

        public GetArticleByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}