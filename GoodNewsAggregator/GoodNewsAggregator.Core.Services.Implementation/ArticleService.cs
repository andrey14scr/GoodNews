using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.DAL.Repositories.Implementation;
using GoodNewsAggregator.DAL.Repositories.Interfaces;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsAggregator.Core.Services.Implementation
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ArticleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task AddArticle(ArticleDto articleDto)
        {
            throw new NotImplementedException();
        }

        public Task AddRange(IEnumerable<ArticleDto> articleDtos)
        {
            throw new NotImplementedException();
        }

        public Task<int> Delete(ArticleDto articleDto)
        {
            throw new NotImplementedException();
        }

        public Task<int> EditArticle(ArticleDto articelDto)
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
