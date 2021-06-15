using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsAggregator.Core.Services.Interfaces
{
    public interface IService<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(Guid id);

        Task Add(T obj);
        Task AddRange(IEnumerable<T> objs);

        Task Update(T obj);

        Task Remove(T obj);
        Task RemoveRange(IEnumerable<T> objs);
    }
}
