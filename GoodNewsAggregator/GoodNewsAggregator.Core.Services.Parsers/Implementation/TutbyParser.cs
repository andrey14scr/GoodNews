using GoodNewsAggregator.Core.Services.Interfaces;
using HtmlAgilityPack;

namespace GoodNewsAggregator.Core.Services.Parsers.Implementation
{
    public class TutbyParser : IWebPageParser
    {
        public string Parse(string url)
        {
            var web = new HtmlWeb();

            var htmlDoc = web.Load(url);

            if (htmlDoc == null)
                return null;

            var node = htmlDoc.GetElementbyId("article_body");

            if (node == null || node.InnerHtml == "")
                return null;

            return node.InnerHtml;
        }
    }
}
