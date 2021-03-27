using GoodNewsAggregator.Core.DTO;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsAggregator.Core.Services.Interfaces
{
    public interface IRssService
    {
        Task<IEnumerable<RssDto>> FindSourse();
        Task<RssDto> GetSourseById(Guid id);

        Task<int> AddSourse(RssDto sourceDto);
        Task<IEnumerable<RssDto>> AddRange(IEnumerable<RssDto> sourceDtos);

        Task<int> EditSourse(RssDto article);
        Task<int> Delete(RssDto article);
    }
}
