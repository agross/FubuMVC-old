using System.Collections.Generic;
using AltOxite.Core.Domain;
using FubuMVC.Core.Controller.Config;
using Microsoft.Practices.ServiceLocation;
using FubuMVC.Core.Html;
using FubuMVC.Core.Html.Expressions;
using FubuMVC.Core.View.WebForms;

namespace AltOxite.Core.Web.Html
{
    public static class HtmlExtensions
    {
        public static LinkExpression SkinCSS(this IAltOxitePage viewPage, string url)
        {
            var siteConfig = ServiceLocator.Current.GetInstance<SiteConfiguration>();
            var baseUrl = siteConfig.CssPath;
            return viewPage.CSS(url).BasedAt(baseUrl);
        }

        public static ScriptReferenceExpression SkinScript(this IAltOxitePage viewPage, string url)
        {
            var siteConfig = ServiceLocator.Current.GetInstance<SiteConfiguration>();
            var baseUrl = siteConfig.ScriptsPath;
            return viewPage.Script(url).BasedAt(baseUrl);
        }

        public static ScriptReferenceExpression SkinScript(this IAltOxitePage viewPage, IEnumerable<string> urls)
        {
            var siteConfig = ServiceLocator.Current.GetInstance<SiteConfiguration>();
            var baseUrl = siteConfig.ScriptsPath;
            return viewPage.Script(urls).BasedAt(baseUrl);
        }

        public static LoginStatusExpression DisplayDependingOnLoginStatus(this IAltOxitePage viewPage)
        {
            var renderer = ServiceLocator.Current.GetInstance<IWebFormsViewRenderer>();
            var conventions = ServiceLocator.Current.GetInstance<FubuConventions>();
            return new LoginStatusExpression(viewPage, renderer, conventions);
        }
    }
}