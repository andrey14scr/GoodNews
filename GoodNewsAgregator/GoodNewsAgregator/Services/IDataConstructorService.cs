using GoodNewsAgregator.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsAgregator.Services
{
    public interface IDataConstructorService
    {
        IEnumerable<Article> GetArticles(int amount);
    }
}
