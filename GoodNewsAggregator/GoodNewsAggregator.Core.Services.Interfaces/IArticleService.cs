using GoodNewsAggregator.Core.DTO;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsAggregator.Core.Services.Interfaces
{
    public interface IArticleService : IService<ArticleDto>
    {
        Task<IEnumerable<ArticleDto>> GetByRssId(Guid id);
        Task<IEnumerable<ArticleDto>> GetArticleInfoFromRss(RssDto rss);
    }
}
