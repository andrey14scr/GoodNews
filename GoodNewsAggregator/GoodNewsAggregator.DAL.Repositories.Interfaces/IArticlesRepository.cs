using GoodNewsAggregator.DAL.Core;

using System;
using System.Collections.Generic;
using System.Text;

namespace GoodNewsAggregator.Core.Services.Interfaces
{
    public interface IArticlesRepository : IRepository<Article>
    {
        //for unique methods
    }
}
