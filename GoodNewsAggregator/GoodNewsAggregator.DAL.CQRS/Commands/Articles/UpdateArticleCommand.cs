﻿using System;
using GoodNewsAggregator.Core.DTO;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Commands.Articles
{
    public class UpdateArticleCommand : IRequest<int>
    {
        public Guid Id { get; set; }
        public string Source { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public float? GoodFactor { get; set; }
        public Guid RssId { get; set; }

        public UpdateArticleCommand(ArticleDto articleDto)
        {
            Id = articleDto.Id;
            Source = articleDto.Source;
            Title = articleDto.Title;
            Content = articleDto.Content;
            Date = articleDto.Date;
            GoodFactor = articleDto.GoodFactor;
            RssId = articleDto.RssId;
        }
    }
}