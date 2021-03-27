using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Text;

namespace GoodNewsAggregator.DAL.Repositories.Implementation
{
    public class CommentsRepository : Repository<Comment>
    {
        public CommentsRepository(GoodNewsAggregatorContext context, DbSet<Comment> table) : base(context, table)
        { }
    }
}
