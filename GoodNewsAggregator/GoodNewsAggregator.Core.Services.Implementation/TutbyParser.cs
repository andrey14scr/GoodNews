using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using GoodNewsAggregator.Core.Services.Interfaces;

namespace GoodNewsAggregator.Core.Services.Implementation
{
    public class TutbyParser : IWebPageParser
    {
        public string Parse(string url)
        {
            return "tutby content";

            var web = new HtmlWeb();

            var htmlDoc = web.Load(url);

            if (htmlDoc == null)
                return null;

            var node = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='b-article']"); //b-article

            if (node == null || node.InnerHtml == "")
                return null;

            return node.InnerHtml;
        }
    }
}
