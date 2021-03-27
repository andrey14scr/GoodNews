using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.DAL.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsAggregator.DAL.Repositories.Implementation
{
    public class SourcesRepository : IRepository<Source>
    {
        private readonly GoodNewsAggregatorContext _context;

        public SourcesRepository(GoodNewsAggregatorContext context)
        {
            _context = context;
        }

        public Task Add(Source news)
        {
            throw new NotImplementedException();
        }

        public Task AddRange(IEnumerable<Source> sources)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Source> FindBy(Expression<Func<Source, bool>> predicate, params Expression<Func<Source, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<Source> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task Remove(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task RemoveRange(IEnumerable<Source> sources)
        {
            throw new NotImplementedException();
        }

        public Task Update(Source news)
        {
            throw new NotImplementedException();
        }
    }
}
