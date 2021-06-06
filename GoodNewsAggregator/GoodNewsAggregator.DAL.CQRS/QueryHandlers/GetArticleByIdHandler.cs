using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.CQRS.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GoodNewsAggregator.DAL.CQRS.QueryHandlers
{
    public class GetArticleByIdHandler : IRequestHandler<GetArticleByIdQuery, ArticleDto>
    {
        private readonly GoodNewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public GetArticleByIdHandler(GoodNewsAggregatorContext dvContext)
        {
            _dbContext = dvContext;
        }

        public async Task<ArticleDto> Handle(GetArticleByIdQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<ArticleDto>(await _dbContext.Articles.FirstOrDefaultAsync(a => a.Id.Equals(request.Id)));
        }
    }
}