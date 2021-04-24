using GoodNewsAggregator.DAL.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GoodNewsAggregator.DAL.Core.Entities;

namespace GoodNewsAggregator.DAL.Repositories.Interfaces
{
    public interface IRepository<T> : IDisposable where T : class, IBaseEntity
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(Guid id);

        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        IQueryable<T> Get();

        Task Add(T obj);
        Task AddRange(IEnumerable<T> objs);

        Task Update(T obj);
        Task UpdateRange(IEnumerable<T> objs);

        Task Remove(Guid id);
        Task Remove(T obj);
        Task RemoveRange(IEnumerable<T> objs);
    }
}
