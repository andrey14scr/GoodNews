using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.CQRS.Commands.Articles;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.CommandHandlers.Articles
{
    public class AddArticleHandler : IRequestHandler<AddArticleCommand, int>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public AddArticleHandler(GoodNewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(AddArticleCommand request, CancellationToken cancellationToken)
        {
            var article = _mapper.Map<Article>(request);
            await _dbContext.Articles.AddAsync(article);
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}