using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Repositories.Interfaces;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsAggregator.DAL.Repositories.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GoodNewsAggregatorContext _db;
        private readonly IRepository<Article> _newsRepository;
        private readonly IRepository<Rss> _rssRepository;


        public UnitOfWork(GoodNewsAggregatorContext db,
            IRepository<Article> newsRepository,
            IRepository<Rss> rssRepository)
        {
            _db = db;
            _newsRepository = newsRepository;
            _rssRepository = rssRepository;
        }

        public IRepository<Article> News => _newsRepository;
        public IRepository<Rss> RssSources => _rssRepository;

        public IRepository<Article> Article { get; }
        public IRepository<Rss> Rss { get; }

        public async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db?.Dispose();
            GC.SuppressFinalize(this);
        }

        Task<int> IUnitOfWork.SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
