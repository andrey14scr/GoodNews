using System.Collections.Generic;
using GoodNewsAggregator.Core.DTO;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Commands.Comments
{
    public class UpdateCommentsRangeCommand : IRequest<int>
    {
        public IEnumerable<CommentDto> CommentDtos { get; set; }

        public UpdateCommentsRangeCommand(IEnumerable<CommentDto> commentDtos)
        {
            CommentDtos = commentDtos;
        }
    }
}