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
                    result += "<p>" + node.InnerHtml + "</p>";
                }
                else if (node.Name == "h2" && node.ChildNodes[0].Name != "a")
                {
                    result += "<h2>" + node.InnerHtml + "</h2>";
                }
                else if (node.Name == "h3")
                {
                    result += "<h3>" + node.InnerHtml + "</h3>";
                }
                else if (node.Name == "ul")
                {
                    result += "<ul>" + node.InnerHtml + "</ul>";
                }
                else if (node.Name == "div" && node.HasClass("news-media") )
                {
                    var temp = node.FirstChild;
                    if (temp != null && temp.HasClass("news-media__inside"))
                    {
                        temp = temp.FirstChild;
                        if (temp != null && temp.HasClass("news-media__viewport"))
                        {
                            temp = temp.FirstChild;
                            if (temp != null && temp.HasClass("news-media__preview"))
                            {
                                temp = temp.FirstChild;
                                if (temp != null && temp.Name == "img")
                                {
                                    result += "<p>" + temp.ParentNode.InnerHtml + "</p>";
                                }
                            }
                        }
                    }
                }

            }
            
            if (string.IsNullOrWhiteSpace(result))
                return null;

            return result;
        }
    }
}
