using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using GoodNewsAggregator.Models;
using GoodNewsAggregator.Views.Constants;
using Microsoft.AspNetCore.Mvc.TagHelpers;

namespace GoodNewsAggregator.TagHelpers
{
    public class PaginationTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;

        public PaginationTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        public PageInfo Page { get; set; }
        public string PageAction { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
            var result = new TagBuilder("div");
            string anchorInnerHtml;

            for (int i = 1; i <= Page.TotalPages; i++)
            {
                var tag = new TagBuilder("a");
                anchorInnerHtml = GetAnchorInnerHtml(i, Page);
                if (anchorInnerHtml == Pagination.POINTS)
                {
                    tag.InnerHtml.Append(anchorInnerHtml);
                    tag.AddCssClass("btn btn-outline-dark mt-3 ml-1");
                    result.InnerHtml.AppendHtml(tag);
                    continue;
                }
                else if (string.IsNullOrEmpty(anchorInnerHtml))
                {
                    continue;
                }
                tag.Attributes["href"] = urlHelper.Action(PageAction, new { pageNumber = i});
                tag.InnerHtml.Append(anchorInnerHtml);
                if(i == Page.PageNumber)
                    tag.AddCssClass("btn btn-dark mt-3 ml-1");
                else
                    tag.AddCssClass("btn btn-outline-dark mt-3 ml-1");
                result.InnerHtml.AppendHtml(tag);
            }

            output.Content.AppendHtml(result.InnerHtml);
        }

        public string GetAnchorInnerHtml(int i, PageInfo page)
        {
            if (page.TotalPages < 10)
            {
                return i.ToString();
            }
            else 
            {
                if (page.PageNumber <= 5)
                {
                    if (i <= page.PageNumber + 2 || i == page.TotalPages)
                        return i.ToString();
                    else if (i == page.PageNumber + 3)
                        return Pagination.POINTS;
                }
                else if(page.PageNumber >= page.TotalPages - 4)
                {
                    if (i == 1 || i >= page.PageNumber - 2)
                        return i.ToString();
                    else if (i == 2)
                        return Pagination.POINTS;
                }
                else
                {
                    if (i == 1 || i == page.TotalPages || Math.Abs(page.PageNumber-i) < 3)
                        return i.ToString();
                    else if (i == 2 || i == page.TotalPages - 1)
                        return Pagination.POINTS;
                }

                return string.Empty;
            }
        }
    }
}
