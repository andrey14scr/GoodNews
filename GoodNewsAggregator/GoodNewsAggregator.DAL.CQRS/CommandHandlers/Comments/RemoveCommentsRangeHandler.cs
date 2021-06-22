using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.CQRS.Commands.Comments;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.CommandHandlers.Comments
{
    public class RemoveCommentsRangeHandler : IRequestHandler<RemoveCommentsRangeCommand, int>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public RemoveCommentsRangeHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(RemoveCommentsRangeCommand request, CancellationToken cancellationToken)
        {
            _dbContext.Comments.RemoveRange(_mapper.Map<IEnumerable<Comment>>(request.CommentDtos));
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}