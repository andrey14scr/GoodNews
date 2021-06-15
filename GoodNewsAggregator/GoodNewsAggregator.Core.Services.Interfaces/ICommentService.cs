using GoodNewsAggregator.Core.DTO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoodNewsAggregator.DAL.Core.Entities;

namespace GoodNewsAggregator.Core.Services.Interfaces
{
    public interface ICommentService : IService<CommentDto>
    {
        Task<IEnumerable<CommentDto>> GetByArticleId(Guid id);
    }
}
