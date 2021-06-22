using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.CQRS.Commands.Comments;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.CommandHandlers.Comments
{
    public class RemoveCommentHandler : IRequestHandler<RemoveCommentCommand, int>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public RemoveCommentHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(RemoveCommentCommand request, CancellationToken cancellationToken)
        {
            _dbContext.Comments.Remove(_mapper.Map<Comment>(request.CommentDto));
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}