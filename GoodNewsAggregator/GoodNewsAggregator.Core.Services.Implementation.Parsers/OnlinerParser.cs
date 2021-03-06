﻿using System.Linq;
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

            if (value == null || value.InnerHtml == string.Empty)
                return null;

            string result = string.Empty;

            foreach (var node in value.ChildNodes)
            {
                if (node.Name == "p" && (node.InnerText.Contains("Читайте также:") || node.InnerText.Contains("Пишите нам:")))
                    break;

                if (node.Name == "p" && node.Attributes.Count == 0 ||
                    node.Name == "h2" && node.ChildNodes[0].Name != "a" && !node.InnerHtml.Contains("catalog.onliner.by") ||
                    node.Name == "h3" || node.Name == "h4" || node.Name == "h5" || node.Name == "h6" ||
                    node.Name == "ul")
                {
                    result += $"<{node.Name}> {HtmlWithoutClasses(node)} </{node.Name}>";
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
                                    result += $"<p> {HtmlWithoutClasses(temp)} </p>";
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

        private string HtmlWithoutClasses(HtmlNode node)
        {
            if (node.Name == "img")
            {
                node.Attributes.Remove("class");
                node.Attributes.Remove("id");

                return node.OuterHtml;
            }

            foreach (var c in node.Attributes.ToArray())
            {
                node.Attributes.Remove(c.Name);
            }

            return node.InnerHtml;
        }
    }
}
