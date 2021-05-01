using GoodNewsAggregator.Core.Services.Interfaces;
using HtmlAgilityPack;

namespace GoodNewsAggregator.Core.Services.Implementation.Parsers
{
    public class S13Parser : IWebPageParser
    {
        public string Parse(string url)
        {
            return "s13 content";

            var web = new HtmlWeb();

            var htmlDoc = web.Load(url);

            if (htmlDoc == null)
                return null;

            var nodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='content']");

            if (nodes == null)
                return null;

            var node = nodes[nodes.Count-1];

            string res = "";

            foreach (var block in node.ChildNodes)
            {
                if (block.Name == "p" || block.Name == "blockquote")
                {
                    res += block.InnerHtml;
                }
            }

            return res;
        }
    }
}
