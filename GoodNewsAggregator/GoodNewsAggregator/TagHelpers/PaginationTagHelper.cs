using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using GoodNewsAggregator.Models;
using Microsoft.Extensions.Configuration;

namespace GoodNewsAggregator.TagHelpers
{
    public class PaginationTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IConfiguration _configuration;

        public PaginationTagHelper(IUrlHelperFactory urlHelperFactory, IConfiguration configuration)
        {
            _urlHelperFactory = urlHelperFactory;
            _configuration = configuration;
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

                if (string.IsNullOrEmpty(anchorInnerHtml))
                    continue;

                tag.InnerHtml.Append(anchorInnerHtml);

                if (anchorInnerHtml == _configuration["Constants:PaginationDelimiter"])
                {
                    tag.AddCssClass("btn btn-outline-dark mt-3 ml-1 disabled");
                    result.InnerHtml.AppendHtml(tag);
                    continue;
                }
                
                tag.Attributes["href"] = urlHelper.Action(PageAction, new { page = i, sortBy = Page.SortByOption});
                if (i == Page.PageNumber)
                    tag.AddCssClass("current-pagination-link btn btn-dark mt-3 ml-1");
                else
                    tag.AddCssClass("pagination-link btn btn-outline-dark mt-3 ml-1");
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

            if (page.PageNumber <= 5)
            {
                if (i <= page.PageNumber + 2 || i == page.TotalPages)
                    return i.ToString();
                if (i == page.PageNumber + 3)
                    return _configuration["Constants:PaginationDelimiter"];
            }
            else if(page.PageNumber >= page.TotalPages - 4)
            {
                if (i == 1 || i >= page.PageNumber - 2)
                    return i.ToString();
                if (i == 2)
                    return _configuration["Constants:PaginationDelimiter"];
            }
            else
            {
                if (i == 1 || i == page.TotalPages || Math.Abs(page.PageNumber-i) < 3)
                    return i.ToString();
                if (i == 2 || i == page.TotalPages - 1)
                    return _configuration["Constants:PaginationDelimiter"];
            }

            return string.Empty;
        }
    }
}
