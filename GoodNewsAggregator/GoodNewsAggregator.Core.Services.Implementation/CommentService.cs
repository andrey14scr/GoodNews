using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsAggregator.Core.Services.Implementation
{
    public class CommentService : ICommentService
    {
        public Task Add(CommentDto comment)
        {
            throw new NotImplementedException();
        }

        public Task AddRange(IEnumerable<CommentDto> comments)
        {
            throw new NotImplementedException();
        }

        public Task<int> Delete(CommentDto comment)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CommentDto>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<CommentDto> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(CommentDto comment)
        {
            throw new NotImplementedException();
        }
    }
}
