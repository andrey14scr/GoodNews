using AutoMapper;

using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.DAL.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsAggregator.Core.Services.Implementation
{
    public class AutoMap : Profile
    {
        public AutoMap()
        {
            CreateMap<Article, ArticleDto>().ReverseMap();
            CreateMap<Article, ArticleWithRssDto>().ReverseMap();
            CreateMap<ArticleDto, ArticleWithRssDto>().ReverseMap();
            CreateMap<Comment, CommentDto>().ReverseMap();
            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<Rss, RssDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
