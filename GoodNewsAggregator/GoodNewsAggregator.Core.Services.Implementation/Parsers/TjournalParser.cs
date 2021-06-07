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

            string result = "";

            foreach (var node in value.ChildNodes)
            {
                if (node.Name == "div" && node.Attributes.Count > 0 && node.Attributes[0].Value == "l-island-a")
                {
                    var paragraph = node.ChildNodes.FirstOrDefault(n => n.Name == "p" || n.Name == "ul");
                    if (paragraph != null)
                    {
                        result += HtmlWithoutClasses(node);
                    }
                    else if (node.ParentNode.Name == "h2" || node.ParentNode.Name == "h3")
                    {
                        result += $"<{node.ParentNode.Name}> {node.InnerText} </{node.ParentNode.Name}>";
                    }
                }
                else if (node.Name == "figure")
                {
                    string r = "";
                    FindImage(node, ref r);
                    result += r;
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

        private static void FindImage(HtmlNode node, ref string res)
        {
            foreach (var n in node.ChildNodes)
            {
                if (n.Name == "div" && n.HasClass("andropov_image"))
                {
                    string a = "";
                    string imgSource = n.GetAttributeValue("data-image-src", a);
                    if (!string.IsNullOrWhiteSpace(imgSource))
                        res = $"<img src=\"{imgSource}\">";
                    return;
                }

                FindImage(n, ref res);
            }
        }
    }
}
