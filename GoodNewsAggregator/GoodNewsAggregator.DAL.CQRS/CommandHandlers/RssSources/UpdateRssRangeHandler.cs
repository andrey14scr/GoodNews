using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.CQRS.Commands.RssSources;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.CommandHandlers.RssSources
{
    public class UpdateRssRangeHandler : IRequestHandler<UpdateRssRangeCommand, int>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public UpdateRssRangeHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(UpdateRssRangeCommand request, CancellationToken cancellationToken)
        {
            var rss = _mapper.Map<IEnumerable<Rss>>(request.RssDtos);
            _dbContext.Rss.UpdateRange(rss);
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}