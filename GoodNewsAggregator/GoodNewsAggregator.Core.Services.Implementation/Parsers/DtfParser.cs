using System.Linq;
using GoodNewsAggregator.Core.Services.Interfaces;
using HtmlAgilityPack;

namespace GoodNewsAggregator.Core.Services.Implementation.Parsers
{
    public class DtfParser : IWebPageParser
    {
        public string Parse(string url)
        {
            var web = new HtmlWeb();

            var htmlDoc = web.Load(url);

            var value = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='content content--full ']");

            if (value == null || value.InnerHtml == "")
                return null;

            string result = "";

            var nodes = value.SelectNodes("//div[@class='l-island-a']");

            foreach (var node in nodes)
            {
                if (node.Name == "div" && node.Attributes.Count > 0 && node.Attributes[0].Value == "l-island-a")
                {
                    var paragraph = node.ChildNodes.FirstOrDefault(n => n.Name == "p" || n.Name == "ul");
                    if (paragraph != null)
                    {
                        result += node.InnerHtml;
                    }
                    else if (node.ParentNode.Name == "h2" || node.ParentNode.Name == "h3")
                    {
                        result += $"<{node.ParentNode.Name}>{node.InnerText}</{node.ParentNode.Name}>";
                    }
                }
            }

            return result;
        }
    }
}
