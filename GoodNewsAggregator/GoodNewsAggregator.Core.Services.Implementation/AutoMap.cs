using AutoMapper;

using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.DAL.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.CQRS.Commands.Articles;

namespace GoodNewsAggregator.Core.Services.Implementation
{
    public class AutoMap : Profile
    {
        public AutoMap()
        {
            CreateMap<Article, ArticleDto>().ReverseMap();
            CreateMap<ArticleWithRssNameDto, ArticleDto>().ReverseMap();
            CreateMap<ArticleWithRssNameDto, Article>().ReverseMap();
            CreateMap<Article, ArticleWithRssDto>().ReverseMap();
            CreateMap<ArticleDto, ArticleWithRssDto>().ReverseMap();
            CreateMap<Comment, CommentDto>().ReverseMap();
            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<Rss, RssDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();

            CreateMap<Article, UpdateArticleCommand>().ReverseMap();
            CreateMap<Article, AddArticleCommand>().ReverseMap();
        }
    }
}
