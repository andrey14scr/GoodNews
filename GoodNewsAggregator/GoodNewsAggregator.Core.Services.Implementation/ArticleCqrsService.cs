using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.CQRS.Commands.Articles;
using GoodNewsAggregator.DAL.CQRS.Queries.Articles;
using MediatR;

namespace GoodNewsAggregator.Core.Services.Implementation
{
    public class ArticleCqrsService : IArticleService
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediatr;

        public ArticleCqrsService(IMapper mapper, IMediator mediatr)
        {
            _mapper = mapper;
            _mediatr = mediatr;
        }

        public async Task<IEnumerable<ArticleDto>> GetAll()
        {
            return await _mediatr.Send(new GetAllArticlesQuery());
        }

        public async Task<ArticleDto> GetById(Guid id)
        {
            return await _mediatr.Send(new GetArticleByIdQuery(id));
        }

        public async Task Add(ArticleDto articleDto)
        {
            await _mediatr.Send(new AddArticleCommand(articleDto));
        }

        public async Task AddRange(IEnumerable<ArticleDto> articleDtos)
        {
            await _mediatr.Send(new AddArticlesRangeCommand(articleDtos));
        }

        public async Task Update(ArticleDto articleDto)
        {
            await _mediatr.Send(new UpdateArticleCommand(articleDto));
        }

        public async Task Remove(ArticleDto articleDto)
        {
            await _mediatr.Send(new RemoveArticleCommand(articleDto.Id));
        }

        public async Task RemoveRange(IEnumerable<ArticleDto> articleDtos)
        {
            await _mediatr.Send(new RemoveArticlesRangeCommand(articleDtos.Select(a => a.Id)));
        }

        public async Task<IEnumerable<ArticleDto>> GetByRssId(Guid id)
        {
            return await _mediatr.Send(new GetArticlesByRssIdQuery(id));
        }

        public async Task<IQueryable<ArticleDto>> Get()
        {
            return _mapper.Map<IQueryable<ArticleDto>>(await _mediatr.Send(new GetArticlesQuery()));
        }

        public async Task AggregateNews()
        {
            await _mediatr.Send(new AggregateNewsCommand());
        }

        public async Task RateNews()
        {
            await _mediatr.Send(new RateNewsCommand());
        }
    }
}