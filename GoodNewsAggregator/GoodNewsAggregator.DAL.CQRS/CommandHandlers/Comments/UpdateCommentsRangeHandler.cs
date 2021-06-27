using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.CQRS.Commands.Comments;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.CommandHandlers.Comments
{
    public class UpdateCommentsRangeHandler : IRequestHandler<UpdateCommentsRangeCommand, int>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public UpdateCommentsRangeHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(UpdateCommentsRangeCommand request, CancellationToken cancellationToken)
        {
            var comments = _mapper.Map<IEnumerable<Comment>>(request.CommentDtos);
            _dbContext.Comments.UpdateRange(comments);
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}