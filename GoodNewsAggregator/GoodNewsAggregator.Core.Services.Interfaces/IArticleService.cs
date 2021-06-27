using GoodNewsAggregator.Core.DTO;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.Services.Interfaces.Enums;

namespace GoodNewsAggregator.Core.Services.Interfaces
{
    public interface IArticleService : IService<ArticleDto>
    {
        Task<IEnumerable<ArticleDto>> GetByRssId(Guid id);
        Task<IEnumerable<ArticleDto>> GetFirst(int skip, int take, bool hasNulls, SortByOption sortByOption);
        Task AggregateNews();
        Task RateNews();
        Task<int> GetArticlesCount();
        Task<int> GetRatedArticlesCount();
    }
}
