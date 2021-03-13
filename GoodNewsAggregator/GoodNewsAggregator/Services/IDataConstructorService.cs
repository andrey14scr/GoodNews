using GoodNewsAggregator.DAL.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsAggregator.Services
{
    public interface IDataConstructorService
    {
        IEnumerable<Article> GetArticles(int amount);
    }
}
