using System.Collections.Generic;
using GoodNewsAggregator.Core.DTO;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Commands.Comments
{
    public class RemoveCommentsRangeCommand : IRequest<int>
    {
        public IEnumerable<CommentDto> CommentDtos { get; set; }

        public RemoveCommentsRangeCommand(IEnumerable<CommentDto> commentDtos)
        {
            CommentDtos = commentDtos;
        }
    }
}