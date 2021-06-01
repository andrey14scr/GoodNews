using System.Linq;
using GoodNewsAggregator.Core.Services.Interfaces;
using HtmlAgilityPack;

namespace GoodNewsAggregator.Core.Services.Implementation.Parsers
{
    public class TjournalParser : IWebPageParser
    {
        public string Parse(string url)
        {
            var web = new HtmlWeb();

            var htmlDoc = web.Load(url);

            if (htmlDoc == null)
                return null;

            var nodes = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='content content--full ']");

            if (nodes == null)
                return null;

            string result = "";

            foreach (var node in nodes.ChildNodes)
            {
                if (node.Name == "div")
                {
                    var paragraph = node.ChildNodes.FirstOrDefault(n => n.Name == "p");
                    if (paragraph != null)
                    {
                        result += "<p>" + paragraph.InnerText + "</p>";
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(result))
                return null;

            return result;
        }
    }
}
