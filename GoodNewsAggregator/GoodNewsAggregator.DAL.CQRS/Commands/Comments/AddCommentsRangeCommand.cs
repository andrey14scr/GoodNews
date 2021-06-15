using System.Collections.Generic;
using GoodNewsAggregator.Core.DTO;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Commands.Comments
{
    public class AddCommentsRangeCommand : IRequest<int>
    {
        public IEnumerable<CommentDto> CommentDtos { get; set; }
    }
}