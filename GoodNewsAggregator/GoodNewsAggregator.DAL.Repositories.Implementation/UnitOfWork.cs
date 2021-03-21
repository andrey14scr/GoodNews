using GoodNewsAggregator.Core.Services.Interfaces;
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
        public IRepository<Article> Article { get; }
        public IRepository<Source> Source { get; }

        private readonly GoodNewsAggregatorContext _db;
        private readonly IRepository<Article> _articlesRepository;
        private readonly IRepository<Source> _sourcesRepository;


        public UnitOfWork(GoodNewsAggregatorContext db, IRepository<Article> newsRepository, IRepository<Source> rssRepository)
        {
            _db = db;
            _articlesRepository = newsRepository;
            _sourcesRepository = rssRepository;
        }

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _db?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
