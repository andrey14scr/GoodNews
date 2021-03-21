using GoodNewsAggregator.Core.Services.Interfaces;
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
        IRepository<Source> Source { get; }

        Task<int> SaveChangesAsync();
    }
}
