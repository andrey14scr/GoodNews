using GoodNewsAggregator.DAL.Core;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GoodNewsAggregator.DAL.Core.Entities;

namespace GoodNewsAggregator.DAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Article> Articles { get; }
        IRepository<Comment> Comments { get; }
        //IRepository<Role> Roles { get; }
        IRepository<Rss> Rss { get; }
        //IRepository<User> Users { get; }

        Task<int> SaveChangesAsync();
    }
}
