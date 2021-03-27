using GoodNewsAggregator.Core.DTO;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsAggregator.DAL.Repositories.Interfaces
{
    public interface ISourceService
    {
        Task<IEnumerable<SourceDto>> FindSourse();
        Task<SourceDto> GetSourseById(Guid id);

        Task<int> AddSourse(SourceDto sourceDto);
        Task<IEnumerable<SourceDto>> AddRange(IEnumerable<SourceDto> sourceDtos);

        Task<int> EditSourse(SourceDto article);
        Task<int> Delete(SourceDto article);
    }
}
