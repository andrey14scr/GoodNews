using GoodNewsAggregator.Core.DTO;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsAggregator.DAL.Repositories.Interfaces
{
    public interface ISourceService
    {
        Task<IEnumerable<SourseDto>> FindSourse();
        Task<SourseDto> GetSourseById(Guid id);

        Task<int> AddSourse(SourseDto news);
        Task<IEnumerable<SourseDto>> AddRange(IEnumerable<SourseDto> news);

        Task<int> EditSourse(SourseDto news);
        Task<int> Delete(SourseDto news);
    }
}
