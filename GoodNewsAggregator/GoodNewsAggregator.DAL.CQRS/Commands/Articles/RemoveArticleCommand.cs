using System;
using GoodNewsAggregator.DAL.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace GoodNewsAggregator.DAL.CQRS.Commands.Articles
{
    public class RemoveArticleCommand : IRequest<int>
    {
        public Guid Id { get; set; }

        public RemoveArticleCommand(Guid id)
        {
            Id = id;
        }
    }
}