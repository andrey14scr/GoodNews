using GoodNewsAggregator.Core.DTO;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodNewsAggregator.Core.Services.Interfaces
{
    public interface ICommentService : IService<CommentDto>
    {
        Task<IEnumerable<CommentDto>> GetByArticleId(Guid id);
    }
}
