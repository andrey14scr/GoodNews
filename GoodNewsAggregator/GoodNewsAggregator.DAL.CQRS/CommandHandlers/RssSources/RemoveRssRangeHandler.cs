using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.CQRS.Commands.Comments;
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
            var rss = _dbContext.Rss.Where(r => request.Ids.Contains(r.Id));
            _dbContext.Rss.RemoveRange(rss);
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}