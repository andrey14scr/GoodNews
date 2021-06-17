using AutoMapper;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.CQRS.Commands.Articles;
using GoodNewsAggregator.DAL.CQRS.Commands.Comments;
using GoodNewsAggregator.DAL.CQRS.Commands.RssSources;
using GoodNewsAggregator.Models;

namespace GoodNewsAggregator.Tools
{
    public class AutoMap : Profile
    {
        public AutoMap()
        {
            CreateMap<ArticleDto, ArticleInfoViewModel>().ReverseMap();

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

            CreateMap<Comment, UpdateCommentCommand>().ReverseMap();
            CreateMap<Comment, AddCommentCommand>().ReverseMap();

            CreateMap<Rss, UpdateRssCommand>().ReverseMap();
            CreateMap<Rss, AddRssCommand>().ReverseMap();
        }
    }
}
