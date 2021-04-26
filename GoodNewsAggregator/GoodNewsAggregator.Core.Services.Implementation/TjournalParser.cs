using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.Services.Interfaces;
using HtmlAgilityPack;

namespace GoodNewsAggregator.Core.Services.Implementation
{
    public class TjournalParser : IWebPageParser
    {
        public string Parse(string url)
        {
            return "tjournal content";

            var web = new HtmlWeb();

            var htmlDoc = web.Load(url);

            if (htmlDoc == null)
                return null;

            var nodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='l-island-a']");

            if (nodes == null)
                return null;

            string res = "";

            foreach (var node in nodes)
            {
                res += node.InnerHtml;
            }

            return res;
        }
    }
}
