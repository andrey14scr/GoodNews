using GoodNewsAggregator.Core.DTO;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsAggregator.Core.Services.Interfaces
{
    public interface IRssService
    {
        Task<IEnumerable<RssDto>> GetAll();
        Task<RssDto> GetById(Guid id);

        Task Add(RssDto rssDto);
        Task AddRange(IEnumerable<RssDto> rssDtos);

        Task Update(RssDto rssDto);

        Task Remove(RssDto rssDto);
        Task RemoveRange(IEnumerable<RssDto> rssDtos);
    }
}
