using GoodNewsAggregator.DAL.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsAggregator.Core.Services.Interfaces
{
    public interface IRepository<T> : IDisposable where T : class, IBaseEntity
    {
        Task<T> GetById(Guid id);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

        Task Add(T news);
        Task AddRange(IEnumerable<T> news);

        Task Update(T news);
        Task Remove(Guid id);
        Task RemoveRange(IEnumerable<T> news);
    }
}
