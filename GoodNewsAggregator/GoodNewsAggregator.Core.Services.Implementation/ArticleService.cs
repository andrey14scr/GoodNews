using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.DAL.Repositories.Interfaces;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsAggregator.Core.Services.Implementation
{
    public class ArticleService : IArticleService
    {
        public Task AddArticle(ArticleDto news)
        {
            throw new NotImplementedException();
        }

        public Task AddRange(IEnumerable<ArticleDto> news)
        {
            throw new NotImplementedException();
        }

        public Task<int> Delete(ArticleDto news)
        {
            throw new NotImplementedException();
        }

        public Task<int> EditArticle(ArticleDto news)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ArticleDto>> FindArticles()
        {
            throw new NotImplementedException();
        }

        public Task<ArticleDto> GetArticleById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ArticleDto>> GetArticlesBySourseId(Guid? id)
        {
            throw new NotImplementedException();
        }
    }
}
