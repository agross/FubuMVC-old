using System.Collections.Generic;
using VsTemplate.Core.Domain;
using FubuMVC.Core.Controller.Config;
using Microsoft.Practices.ServiceLocation;
using FubuMVC.Core.Html;
using FubuMVC.Core.Html.Expressions;
using FubuMVC.Core.View.WebForms;

namespace VsTemplate.Core.Web.Html
{
    public static class HtmlExtensions
    {
        public static LinkExpression SkinCSS(this IFubuMvcPage viewPage, string url)
        {
            var siteConfig = ServiceLocator.Current.GetInstance<SiteConfiguration>();
            var baseUrl = siteConfig.CssPath;
            return viewPage.CSS(url).BasedAt(baseUrl);
        }

        public static ScriptReferenceExpression SkinScript(this IFubuMvcPage viewPage, string url)
        {
            var siteConfig = ServiceLocator.Current.GetInstance<SiteConfiguration>();
            var baseUrl = siteConfig.ScriptsPath;
            return viewPage.Script(url).BasedAt(baseUrl);
        }

        public static ScriptReferenceExpression SkinScript(this IFubuMvcPage viewPage, IEnumerable<string> urls)
        {
            var siteConfig = ServiceLocator.Current.GetInstance<SiteConfiguration>();
            var baseUrl = siteConfig.ScriptsPath;
            return viewPage.Script(urls).BasedAt(baseUrl);
        }
    }
}