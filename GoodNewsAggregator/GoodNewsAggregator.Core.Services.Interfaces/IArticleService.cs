using GoodNewsAggregator.Core.DTO;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsAggregator.Core.Services.Interfaces
{
    public interface IArticleService
    {
        Task<IEnumerable<ArticleDto>> GetAll();
        Task<IEnumerable<ArticleDto>> GetByRssId(Guid id);
        Task<ArticleDto> GetById(Guid id);

        Task Add(ArticleDto article);
        Task AddRange(IEnumerable<ArticleDto> articles);

        Task Update(ArticleDto article);

        Task Remove(ArticleDto article);
    }
}
