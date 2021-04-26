using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.Services.Interfaces;
using HtmlAgilityPack;

namespace GoodNewsAggregator.Core.Services.Implementation
{
    public class DtfParser : IWebPageParser
    {
        public string Parse(string url)
        {
            return "dtf content";

            var web = new HtmlWeb();

            var htmlDoc = web.Load(url);

            var node = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='content content--full ']");

            if (node == null || node.InnerHtml == "")
                return null;

            return node.InnerHtml;
        }
    }
}
