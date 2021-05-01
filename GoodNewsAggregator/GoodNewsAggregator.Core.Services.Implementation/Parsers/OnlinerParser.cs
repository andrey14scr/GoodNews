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

            var node = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='news-text']");
            
            if (node == null || node.InnerHtml == "")
                return null;

            return node.InnerHtml;
        }
    }
}
