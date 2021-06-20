using System;
using GoodNewsAggregator.DAL.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GoodNewsAggregator.DAL.Core
{
    public class GoodNewsAggregatorContext : IdentityDbContext<User, Role, Guid>
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Rss> Rss { get; set; }
        public DbSet<AccessToken> AccessTokens { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public GoodNewsAggregatorContext(DbContextOptions<GoodNewsAggregatorContext> options) : base(options) { }
    }
}
