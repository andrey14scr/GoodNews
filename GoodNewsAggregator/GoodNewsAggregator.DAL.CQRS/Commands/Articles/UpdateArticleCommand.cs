using System;
using System.Collections.Generic;
using GoodNewsAggregator.DAL.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace GoodNewsAggregator.DAL.CQRS.Commands.Articles
{
    public class UpdateArticleCommand : IRequest<int>
    {
        public Guid Id { get; set; }
        public string Source { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public float GoodFactor { get; set; }
        public Guid RssId { get; set; }
    }
}