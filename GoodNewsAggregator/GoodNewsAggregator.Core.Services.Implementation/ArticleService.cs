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
using System.Xml;
using AutoMapper;
using GoodNewsAggregator.DAL.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Terradue.ServiceModel.Syndication;

namespace GoodNewsAggregator.Core.Services.Implementation
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ArticleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Add(ArticleDto commentDto)
        {
            await AddRange(new[] { commentDto });
        }

        public async Task AddRange(IEnumerable<ArticleDto> articleDtos)
        {
            var articles = _mapper.Map<List<Article>>(articleDtos.ToList());

            await _unitOfWork.Articles.AddRange(articles);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Remove(ArticleDto articleDto)
        {
            await RemoveRange(new[] { articleDto });
        }

        public async Task RemoveRange(IEnumerable<ArticleDto> articleDtos)
        {
            var articles = _mapper.Map<List<Article>>(articleDtos.ToList());
            await _unitOfWork.Articles.RemoveRange(articles);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Update(ArticleDto articleDto)
        {
            var article = _mapper.Map<Article>(articleDto);
            await _unitOfWork.Articles.Update(article);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<ArticleDto>> GetAll()
        {
            var articles = await _unitOfWork.Articles.GetAll();
            var articleDtos = _mapper.Map<List<ArticleDto>>(articles);
            
            return articleDtos;
        }

        public async Task<ArticleDto> GetById(Guid id)
        {
            var article = await _unitOfWork.Articles.GetById(id);
            var articleDto = _mapper.Map<ArticleDto>(article);
            
            return articleDto;
        }

        public async Task<IEnumerable<ArticleDto>> GetByRssId(Guid id)
        {
            var articles = await _unitOfWork.Articles.Get().Where(a => a.Rss.Id.Equals(id)).ToListAsync();
            var articleDtos = _mapper.Map<List<ArticleDto>>(articles);

            return articleDtos;
        }

        public async Task<IEnumerable<ArticleDto>> GetArticleInfosFromRss(RssDto rss)
        {
            var articleDtos = new List<ArticleDto>();

            using (var reader = XmlReader.Create(rss.Url))
            {
                var feed = SyndicationFeed.Load(reader);
                reader.Close();
                if (feed.Items.Any())
                {
                    var currentArticleUrls = await _unitOfWork.Articles
                        .Get()
                        .Select(a => a.Source)
                        .ToListAsync();

                    Parallel.ForEach(feed.Items, syndicationItem =>
                    {
                        if (!currentArticleUrls.Any(url => url.Equals(syndicationItem.Id)))
                        {
                            var newsDto = new ArticleDto()
                            {
                                Id = Guid.NewGuid(),
                                RssId = rss.Id,
                                Source = syndicationItem.Links[0].Uri.ToString(),
                                Title = syndicationItem.Title.Text,
                                Date = syndicationItem.PublishDate.DateTime
                            };

                            articleDtos.Add(newsDto);
                        }
                    });
                }
            }

            return articleDtos;
        }

        public IQueryable<Article> Get()
        {
            return _unitOfWork.Articles.Get();
        }
    }
}
