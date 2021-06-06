using AutoMapper;
using GoodNewsAggregator.Core.Services.Interfaces;
using MediatR;

namespace GoodNewsAggregator.Core.Services.Implementation
{
    public class ArticleCqrsService : IArticleCqrsService
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediatr;

        public ArticleCqrsService(IMapper mapper, IMediator mediatr)
        {
            _mapper = mapper;
            _mediatr = mediatr;
        }
    }
}