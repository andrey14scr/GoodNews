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

        Task<int> Add(RssDto rssDto);
        Task<IEnumerable<RssDto>> AddRange(IEnumerable<RssDto> rssDtos);

        Task<int> Update(RssDto rss);

        Task<int> Delete(RssDto rss);
    }
}
