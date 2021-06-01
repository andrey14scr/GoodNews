using System.Collections.Generic;
using System.Linq;
using GoodNewsAggregator.Core.Services.Interfaces;
using HtmlAgilityPack;

namespace GoodNewsAggregator.Core.Services.Implementation.Parsers
{
    public class OnlinerParser : IWebPageParser
    {
        public string Parse(string url)
        {
            var web = new HtmlWeb();

            var htmlDoc = web.Load(url);

            if (htmlDoc == null)
                return null;

            var value = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='news-text']");

            string result = "";

            foreach (var node in value.ChildNodes)
            {
                if (node.Name == "p" && node.Attributes.Count == 0)
                {
                    result += "<p>" + node.InnerText + "</p>";
                }
                else if (node.Name == "h2" && node.ChildNodes[0].Name != "a")
                {
                    result += "<h2>" + node.InnerText + "</h2>";
                }
                else if (node.Name == "h3")
                {
                    result += "<h3>" + node.InnerText + "</h3>";
                }
            }
            
            if (string.IsNullOrWhiteSpace(result))
                return null;

            return result;
        }
    }
}
