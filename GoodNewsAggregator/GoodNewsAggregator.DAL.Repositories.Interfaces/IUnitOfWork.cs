using GoodNewsAggregator.DAL.Core;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsAggregator.DAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Article> Article { get; }
        IRepository<Rss> Rss { get; }

        Task<int> SaveChangesAsync();
    }
}
