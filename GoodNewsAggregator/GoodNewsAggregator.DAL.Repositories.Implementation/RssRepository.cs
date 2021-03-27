using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GoodNewsAggregator.DAL.Repositories.Implementation
{
    public class RssRepository : Repository<Rss>
    {
        public RssRepository(GoodNewsAggregatorContext context, DbSet<Rss> table) : base(context, table)
        { }
    }
}
