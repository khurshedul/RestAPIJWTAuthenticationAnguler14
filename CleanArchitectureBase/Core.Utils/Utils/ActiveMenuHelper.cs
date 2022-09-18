using System;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;


public static class ActiveMenuHelper
{
    public static IUrlHelper GetUrlHelper(this IHtmlHelper html)
    {
        var urlFactory = html.ViewContext.HttpContext.RequestServices.GetRequiredService<IUrlHelperFactory>();
        var actionAccessor = html.ViewContext.HttpContext.RequestServices.GetRequiredService<IActionContextAccessor>();
        return urlFactory.GetUrlHelper(actionAccessor.ActionContext);
    }
    //public static IHtmlContent MenuLink(this IHtmlHelper htmlHelper, string linkText, string controller, string action, string area, string anchorTitle = null, string cssClass = null)
    //{
    //    var currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
    //    var currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");
    //    var currentArea = htmlHelper.ViewContext.RouteData.DataTokens["area"];
    //    var urlHelper = htmlHelper.GetUrlHelper();
    //    var url = urlHelper.Action(action, controller, new { area });
    //    var anchor = new TagBuilder("a");
    //    anchor.InnerHtml.AppendHtml(linkText);
    //    anchor.MergeAttribute("href", url);
    //    anchor.Attributes.Add("title", anchorTitle);
    //    var listItem = new TagBuilder("li");
    //    listItem.InnerHtml.AppendHtml(anchor);
    //    listItem.AddCssClass(cssClass);
    //    if (String.Equals(controller, currentController, StringComparison.CurrentCultureIgnoreCase) && String.Equals(action, currentAction, StringComparison.CurrentCultureIgnoreCase))
    //        listItem.AddCssClass("active");

    //    return listItem;
    //}
    public static string GetRequiredString(this RouteData routeData, string keyName)
    {
        object value;
        if (!routeData.Values.TryGetValue(keyName, out value))
        {
            //throw new InvalidOperationException($"Could not find key with name '{keyName}'");
        }

        return value?.ToString();
    }
    public static IHtmlContent MenuLink(this IHtmlHelper htmlHelper, string linkText, string pageLink, string cssClass = null, string anchorTitle = null)
    {
        var currentPageLink = htmlHelper.ViewContext.RouteData.GetRequiredString("Page");
        var currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
        var currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");
        var currentArea = htmlHelper.ViewContext.RouteData.DataTokens["area"];
        var urlHelper = htmlHelper.GetUrlHelper();
        var url = urlHelper.Action(pageLink);
        var anchor = new TagBuilder("a");
        anchor.InnerHtml.AppendHtml(linkText);
        anchor.MergeAttribute("href", url);
        anchor.Attributes.Add("title", anchorTitle);
        var listItem = new TagBuilder("li");
        listItem.InnerHtml.AppendHtml(anchor);
        listItem.AddCssClass(cssClass);
        if (pageLink.Contains(currentController, StringComparison.CurrentCultureIgnoreCase) && pageLink.Contains( currentAction, StringComparison.CurrentCultureIgnoreCase))
            listItem.AddCssClass("active");
        if (String.Equals(pageLink, currentPageLink, StringComparison.CurrentCultureIgnoreCase))
            listItem.AddCssClass("active");
        return listItem;
    }
}

