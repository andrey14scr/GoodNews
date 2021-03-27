using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces;
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

        public Task<IEnumerable<ArticleDto>> GetArticlesByRssId(Guid? id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ArticleDto> GetRandomArticles(int amount)
        {
            string miniContent = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.";
            List<ArticleDto> list = new List<ArticleDto>();

            for (int i = 0; i < amount; i++)
            {
                list.Add(new ArticleDto { Content = miniContent + i, Date = DateTime.Now, GoodFactor = i * 0.12f, Id = Guid.NewGuid(), RssId = Guid.NewGuid(), Title = $"Title {i}" });
            }

            return list;
        }
    }
}
