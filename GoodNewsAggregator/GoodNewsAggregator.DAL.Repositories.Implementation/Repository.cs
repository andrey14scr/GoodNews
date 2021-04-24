using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GoodNewsAggregator.DAL.Core.Entities;

namespace GoodNewsAggregator.DAL.Repositories.Implementation
{
    public abstract class Repository<T> : IRepository<T> where T : class, IBaseEntity
    {
        protected readonly GoodNewsAggregatorContext Db;
        protected readonly DbSet<T> Table;

        protected Repository(GoodNewsAggregatorContext context)
        {
            Db = context;
            Table = Db.Set<T>();
        }

        public async Task Add(T obj)
        {
            await Table.AddAsync(obj);
        }

        public async Task AddRange(IEnumerable<T> objs)
        {
            await Table.AddRangeAsync(objs);
        }

        public async Task<T> GetById(Guid id)
        {
            return await Table.FirstOrDefaultAsync(t => t.Id.Equals(id));
        }

        public async Task Update(T obj)
        {
            Table.Update(obj);
        }

        public async Task UpdateRange(IEnumerable<T> objs)
        {
            Table.UpdateRange(objs);
        }

        public async Task Remove(Guid id)
        {
            Table.Remove(await GetById(id));
        }

        public async Task Remove(T obj)
        {
            Table.Remove(obj);
        }

        public async Task RemoveRange(IEnumerable<T> objs)
        {
            Table.RemoveRange(objs);
        }

        public void Dispose()
        {
            Db?.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await Table.ToListAsync();
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            //var result = Table.Where(predicate);
            //if (includes.Any())
            //{
            //    result = includes
            //        .Aggregate(result,
            //            (current, include)
            //                => current.Include(include));
            //}

            //return result;

            throw new NotImplementedException();
        }

        public IQueryable<T> Get()
        {
            return Table;
        }
    }
}
