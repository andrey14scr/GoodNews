using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.CQRS.Commands.Comments;
using GoodNewsAggregator.DAL.CQRS.Commands.RssSources;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.CommandHandlers.RssSources
{
    public class UpdateRssHandler : IRequestHandler<UpdateRssHandler, int>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public UpdateRssHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(UpdateRssHandler request, CancellationToken cancellationToken)
        {
            var rss = _mapper.Map<Rss>(request);
            _dbContext.Rss.Update(rss);
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}