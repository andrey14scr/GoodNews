using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Text;

namespace GoodNewsAggregator.DAL.Repositories.Implementation
{
    public class UsersRepository : Repository<User>
    {
        public UsersRepository(GoodNewsAggregatorContext context, DbSet<User> table) : base(context, table)
        { }
    }
}
