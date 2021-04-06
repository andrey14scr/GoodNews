using GoodNewsAggregator.Core.DTO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsAggregator.Core.Services.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDto>> GetAll();
        Task<CommentDto> GetById(Guid id);

        Task Add(CommentDto comment);
        Task AddRange(IEnumerable<CommentDto> comments);

        Task<int> Update(CommentDto comment);

        Task<int> Delete(CommentDto comment);
    }
}
