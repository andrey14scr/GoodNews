using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Text;

namespace GoodNewsAggregator.DAL.Repositories.Implementation
{
    public class RolesRepository : Repository<Role>
    {
        public RolesRepository(GoodNewsAggregatorContext context, DbSet<Role> table) : base(context, table)
        { }
    }
}
