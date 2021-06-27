using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.DAL.CQRS.Commands.Comments;
using GoodNewsAggregator.DAL.CQRS.Queries.Comments;
using MediatR;

namespace GoodNewsAggregator.Core.Services.Implementation
{
    public class CommentCqrsService : ICommentService
    {
        private readonly IMediator _mediator;

        public CommentCqrsService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IEnumerable<CommentDto>> GetAll()
        {
            return await _mediator.Send(new GetAllCommentsQuery());
        }

        public async Task<CommentDto> GetById(Guid id)
        {
            return await _mediator.Send(new GetCommentByIdQuery(id));
        }

        public async Task<IEnumerable<CommentDto>> GetByArticleId(Guid articleId)
        {
            return await _mediator.Send(new GetCommentsByArticleIdQuery(articleId));
        }

        public async Task<IEnumerable<CommentDto>> GetFirst(Guid articleId, int skip, int take)
        {
            return await _mediator.Send(new GetFirstCommentsQuery(articleId, skip, take));
        }

        public async Task Add(CommentDto commentDto)
        {
            await _mediator.Send(new AddCommentCommand(commentDto));
        }

        public async Task AddRange(IEnumerable<CommentDto> commentDtos)
        {
            await _mediator.Send(new AddCommentsRangeCommand(commentDtos));
        }

        public async Task Update(CommentDto commentDto)
        {
            await _mediator.Send(new UpdateCommentCommand(commentDto));
        }

        public async Task Remove(CommentDto commentDto)
        {
            await _mediator.Send(new RemoveCommentCommand(commentDto));
        }

        public async Task RemoveRange(IEnumerable<CommentDto> commentDtos)
        {
            await _mediator.Send(new RemoveCommentsRangeCommand(commentDtos));
        }
    }
}