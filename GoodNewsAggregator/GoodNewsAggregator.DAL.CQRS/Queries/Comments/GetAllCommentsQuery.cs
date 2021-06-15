using System;
using System.Collections.Generic;
using GoodNewsAggregator.Core.DTO;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Queries.Comments
{
    public class GetAllCommentsQuery : IRequest<IEnumerable<CommentDto>>
    {
    }
}