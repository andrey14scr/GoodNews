using System.Threading.Tasks;
using GoodNewsAggregator.DAL.Core.Entities;

namespace GoodNewsAggregator.DAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Article> Articles { get; }
        IRepository<Comment> Comments { get; }
        IRepository<Rss> Rss { get; }

        Task<int> SaveChangesAsync();
    }
}
