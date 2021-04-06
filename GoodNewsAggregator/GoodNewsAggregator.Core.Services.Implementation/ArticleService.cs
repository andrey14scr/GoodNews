using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Repositories.Implementation;
using GoodNewsAggregator.DAL.Repositories.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace GoodNewsAggregator.Core.Services.Implementation
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Mapper _mapper = new Mapper(new MapperConfiguration(mc => mc.CreateMap<Article, ArticleDto>()));
        
        public ArticleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Add(ArticleDto articleDto)
        {
            var article = _mapper.Map<ArticleDto, Article>(articleDto);
            await _unitOfWork.Articles.Add(article);
        }

        public async Task AddRange(IEnumerable<ArticleDto> articleDtos)
        {
            var articles = _mapper.Map<List<ArticleDto>, List<Article>>(articleDtos.ToList());
            await _unitOfWork.Articles.AddRange(articles);
        }

        public async Task Remove(ArticleDto articleDto)
        {
            var article = _mapper.Map<ArticleDto, Article>(articleDto);
            await _unitOfWork.Articles.Remove(article);
        }

        public async Task Update(ArticleDto articleDto)
        {
            var article = _mapper.Map<ArticleDto, Article>(articleDto);
            await _unitOfWork.Articles.Update(article);
        }

        public async Task<IEnumerable<ArticleDto>> GetAll()
        {
            var articles = await _unitOfWork.Articles.Get().ToListAsync();
            var articleDtos = _mapper.Map<List<Article>, List<ArticleDto>>(articles);
            
            return articleDtos;
        }

        public async Task<ArticleDto> GetById(Guid id)
        {
            var article = await _unitOfWork.Articles.GetById(id);
            var articleDtos = _mapper.Map<Article, ArticleDto>(article);
            
            return articleDtos;
        }

        public async Task<IEnumerable<ArticleDto>> GetByRssId(Guid id)
        {
            var articleDtos = _mapper.Map<List<Article>, List<ArticleDto>>(await _unitOfWork.Articles.Get().Where(a => a.Rss.Id.Equals(id)).ToListAsync());

            return articleDtos;
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
