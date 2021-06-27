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

            var value = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='content content--full ']");

            if (value == null || value.InnerHtml == "")
                return null;

            var result = string.Empty;

            foreach (var node in value.ChildNodes)
            {
                if (node.Name == "div" && node.Attributes.Count > 0 && node.Attributes[0].Value == "l-island-a")
                {
                    var paragraph = node.ChildNodes.FirstOrDefault(n => n.Name == "p" || n.Name == "ul");
                    if (paragraph != null)
                    {
                        result += HtmlWithoutClasses(node);
                    }
                    else if (node.ParentNode.Name == "h2" || node.ParentNode.Name == "h3" || 
                             node.ParentNode.Name == "h4" || node.ParentNode.Name == "h5" || 
                             node.ParentNode.Name == "h6")
                    {
                        result += $"<{node.ParentNode.Name}> {node.InnerText} </{node.ParentNode.Name}>";
                    }
                }
                else if (node.Name == "figure")
                {
                    var r = string.Empty;
                    FindImage(node, ref r);
                    result += r;
                    if (string.IsNullOrWhiteSpace(r))
                    {
                        FindQuote(node, ref r);
                        r = $"<blockquote> {r} </blockquote>";
                        result += r;
                    }
                }
                else if (node.Name == "h2" || node.Name == "h3" ||
                         node.Name == "h4" || node.Name == "h5" ||
                         node.Name == "h6")
                {
                    result += $"<{node.Name}> {node.InnerText} </{node.Name}>";
                }
            }

            return result;
        }

        private string HtmlWithoutClasses(HtmlNode node)
        {
            foreach (var c in node.Attributes.ToArray())
            {
                node.Attributes.Remove(c.Name);
            }

            return node.InnerHtml;
        }

        private static void FindQuote(HtmlNode node, ref string res)
        {
            foreach (var n in node.ChildNodes)
            {
                if (n.Name == "p")
                {
                    res += $"\"{n.InnerHtml}\"";
                }
                else if (n.Name == "div" && n.HasClass("block-quote__author-content"))
                {
                    var author = string.Empty;
                    var temp = n.ChildNodes.Where(q => q.HasClass("block-quote__author-name")).FirstOrDefault()?.InnerHtml;
                    if (temp != null)
                    {
                        author += $"<strong> (C) {temp}</strong>";
                    }
                    temp = n.ChildNodes.Where(q => q.HasClass("block-quote__author-job")).FirstOrDefault()?.InnerHtml;
                    if (temp != null)
                    {
                        author += $", {temp}";
                    }

                    res += $"<br><span> {author} </span>";
                }
                else
                {
                    FindQuote(n, ref res);
                }
            }
        }

        private static void FindImage(HtmlNode node, ref string res)
        {
            foreach (var n in node.ChildNodes)
            {
                if (n.Name == "div" && n.HasClass("andropov_image"))
                {
                    var a = string.Empty;
                    var imgSource = n.GetAttributeValue("data-image-src", a);
                    if (!string.IsNullOrWhiteSpace(imgSource))
                        res = $"<p><img src=\"{imgSource}\"></p>";
                    return;
                }

                FindImage(n, ref res);
            }
        }
    }
}
