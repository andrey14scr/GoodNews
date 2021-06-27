using GoodNewsAggregator.Core.DTO;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Commands.Comments
{
    public class RemoveCommentCommand : IRequest<int>
    {
        public CommentDto CommentDto { get; set; }

        public RemoveCommentCommand(CommentDto commentDto)
        {
            CommentDto = commentDto;
        }
    }
}