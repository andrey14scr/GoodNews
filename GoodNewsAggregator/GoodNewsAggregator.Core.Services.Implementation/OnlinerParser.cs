using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.Services.Interfaces;
using HtmlAgilityPack;

namespace GoodNewsAggregator.Core.Services.Implementation
{
    public class OnlinerParser : IWebPageParser
    {
        public string Parse(string url)
        {
            return "onliner content";

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
