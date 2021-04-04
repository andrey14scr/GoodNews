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
        private readonly IRepository<Article> _articlesRepository;
        private readonly IRepository<Rss> _rssRepository;
        private readonly IRepository<Comment> _commentsRepository;
        private readonly IRepository<Role> _rolesRepository;
        private readonly IRepository<User> _usersRepository;


        public UnitOfWork(GoodNewsAggregatorContext db,
            IRepository<Article> articlesRepository,
            IRepository<Rss> rssRepository,
            IRepository<Comment> commentsRepository,
            IRepository<Role> rolesRepository,
            IRepository<User> usersRepository)
        {
            _db = db;
            _articlesRepository = articlesRepository;
            _rssRepository = rssRepository;
            _commentsRepository = commentsRepository;
            _rolesRepository = rolesRepository;
            _usersRepository = usersRepository;
        }

        public IRepository<Article> Articles => _articlesRepository;
        public IRepository<Rss> Rss => _rssRepository;
        public IRepository<Comment> Comments => _commentsRepository;
        public IRepository<Role> Roles => _rolesRepository;
        public IRepository<User> Users => _usersRepository;

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
