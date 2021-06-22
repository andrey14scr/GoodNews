using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.CQRS.Commands.RefreshTokens;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.CommandHandlers.RefreshTokens
{
    public class RemoveRefreshTokenHandler : IRequestHandler<RemoveRefreshTokenCommand, int>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public RemoveRefreshTokenHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(RemoveRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            _dbContext.RefreshTokens.Remove(_mapper.Map<RefreshToken>(request.RefreshToken));
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}