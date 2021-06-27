using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.Core.Services.Interfaces.Enums;
using GoodNewsAggregator.DAL.CQRS.Commands.Articles;
using GoodNewsAggregator.DAL.CQRS.Queries.Articles;
using MediatR;

namespace GoodNewsAggregator.Core.Services.Implementation
{
    public class ArticleCqrsService : IArticleService
    {
        private readonly IMediator _mediator;

        public ArticleCqrsService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IEnumerable<ArticleDto>> GetAll()
        {
            return await _mediator.Send(new GetAllArticlesQuery());
        }

        public async Task<ArticleDto> GetById(Guid id)
        {
            return await _mediator.Send(new GetArticleByIdQuery(id));
        }

        public async Task Add(ArticleDto articleDto)
        {
            await _mediator.Send(new AddArticleCommand(articleDto));
        }

        public async Task AddRange(IEnumerable<ArticleDto> articleDtos)
        {
            await _mediator.Send(new AddArticlesRangeCommand(articleDtos));
        }

        public async Task Update(ArticleDto articleDto)
        {
            await _mediator.Send(new UpdateArticleCommand(articleDto));
        }

        public async Task Remove(ArticleDto articleDto)
        {
            await _mediator.Send(new RemoveArticleCommand(articleDto));
        }

        public async Task RemoveRange(IEnumerable<ArticleDto> articleDtos)
        {
            await _mediator.Send(new RemoveArticlesRangeCommand(articleDtos));
        }

        public async Task<IEnumerable<ArticleDto>> GetByRssId(Guid id)
        {
            return await _mediator.Send(new GetArticlesByRssIdQuery(id));
        }

        public async Task<IEnumerable<ArticleDto>> GetFirst(int skip, int take, bool hasNulls, SortByOption sortByOption)
        {
            return await _mediator.Send(new GetFirstArticlesQuery(skip, take, hasNulls, sortByOption));
        }

        public async Task AggregateNews()
        {
            await _mediator.Send(new AggregateNewsCommand());
        }

        public async Task RateNews()
        {
            await _mediator.Send(new RateNewsCommand());
        }

        public async Task<int> GetArticlesCount()
        {
            return await _mediator.Send(new GetArticlesCountQuery());
        }

        public async Task<int> GetRatedArticlesCount()
        {
            return await _mediator.Send(new GetRatedArticlesCountQuery());
        }
    }
}