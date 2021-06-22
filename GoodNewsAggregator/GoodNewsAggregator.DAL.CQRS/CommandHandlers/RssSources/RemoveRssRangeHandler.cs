using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.CQRS.Commands.RssSources;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.CommandHandlers.RssSources
{
    public class RemoveRssRangeHandler : IRequestHandler<RemoveRssRangeCommand, int>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public RemoveRssRangeHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(RemoveRssRangeCommand request, CancellationToken cancellationToken)
        {
            _dbContext.Rss.RemoveRange(_mapper.Map<IEnumerable<Rss>>(request.RssDtos));
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}