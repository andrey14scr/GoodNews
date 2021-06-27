using System;
using GoodNewsAggregator.Core.DTO;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Commands.RssSources
{
    public class AddRssCommand : IRequest<int>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public AddRssCommand(RssDto rssDto)
        {
            Id = rssDto.Id;
            Name = rssDto.Name;
            Url = rssDto.Url;
        }
    }
}