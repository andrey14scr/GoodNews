using GoodNewsAggregator.Core.DTO;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsAggregator.DAL.Repositories.Interfaces
{
    public interface IArticleService
    {
        Task<IEnumerable<ArticleDto>> FindArticles();
        Task<IEnumerable<ArticleDto>> GetArticlesBySourseId(Guid? id);
        Task<ArticleDto> GetArticleById(Guid id);

        Task AddArticle(ArticleDto news);
        Task AddRange(IEnumerable<ArticleDto> news);

        Task<int> EditArticle(ArticleDto news);
        Task<int> Delete(ArticleDto news);
    }
}
