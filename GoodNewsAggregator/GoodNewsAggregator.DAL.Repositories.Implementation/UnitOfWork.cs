using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Repositories.Interfaces;

using System;
using System.Threading.Tasks;
using GoodNewsAggregator.DAL.Core.Entities;

namespace GoodNewsAggregator.DAL.Repositories.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GoodNewsAggregatorContext _db;
        private readonly IRepository<Article> _articlesRepository;
        private readonly IRepository<Rss> _rssRepository;
        private readonly IRepository<Comment> _commentsRepository;


        public UnitOfWork(GoodNewsAggregatorContext db,
            IRepository<Article> articlesRepository,
            IRepository<Rss> rssRepository,
            IRepository<Comment> commentsRepository//,
        )
        {
            _db = db;
            _articlesRepository = articlesRepository;
            _rssRepository = rssRepository;
            _commentsRepository = commentsRepository;
        }

        public IRepository<Article> Articles => _articlesRepository;
        public IRepository<Rss> Rss => _rssRepository;
        public IRepository<Comment> Comments => _commentsRepository;

        public async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
