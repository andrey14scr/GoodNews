using GoodNewsAggregator.DAL.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsAggregator.Services
{
    public class DataConstructorService : IDataConstructorService
    {
        private static string MiniContent = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.";

        private static IEnumerable<Article> _articles;

        public IEnumerable<Article> GetArticles(int amount)
        {
            if (_articles is null) 
            {
                List<Article> list = new List<Article>();

                for (int i = 0; i < amount; i++)
                {
                    list.Add(new Article { Content = MiniContent + i, Date = DateTime.Now, GoodFactor = i * 0.12f, Id = Guid.NewGuid(), SourceId = (short)i, Title = $"Title {i}" });
                }

                _articles = list;
            }

            return _articles;
        }
    }
}
