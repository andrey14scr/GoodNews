using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.CQRS.Commands.Articles;
using GoodNewsAggregator.DAL.CQRS.Commands.Comments;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.CommandHandlers.Comments
{
    public class AddCommentHandler : IRequestHandler<AddCommentCommand, int>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public AddCommentHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = _mapper.Map<Comment>(request);
            await _dbContext.Comments.AddAsync(comment);
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}