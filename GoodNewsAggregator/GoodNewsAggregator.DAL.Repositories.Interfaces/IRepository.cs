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

        IQueryable<T> Get();

        Task Add(T obj);
        Task AddRange(IEnumerable<T> objs);

        void Update(T obj);
        void UpdateRange(IEnumerable<T> objs);

        Task Remove(Guid id);
        void Remove(T obj);
        void RemoveRange(IEnumerable<T> objs);
    }
}
