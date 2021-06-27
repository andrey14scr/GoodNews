using System;
using GoodNewsAggregator.Core.DTO;
using MediatR;

namespace GoodNewsAggregator.DAL.CQRS.Commands.RssSources
{
    public class UpdateRssCommand : IRequest<int>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public UpdateRssCommand(RssDto rssDto)
        {
            Id = rssDto.Id;
            Name = rssDto.Name;
            Url = rssDto.Url;
        }
    }
}