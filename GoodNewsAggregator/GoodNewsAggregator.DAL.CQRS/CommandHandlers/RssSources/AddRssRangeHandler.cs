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
    public class AddRssRangeHandler : IRequestHandler<AddRssRangeCommand, int>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public AddRssRangeHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(AddRssRangeCommand request, CancellationToken cancellationToken)
        {
            var rss = _mapper.Map<IEnumerable<Rss>>(request.RssDtos);
            await _dbContext.Rss.AddRangeAsync(rss);
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}