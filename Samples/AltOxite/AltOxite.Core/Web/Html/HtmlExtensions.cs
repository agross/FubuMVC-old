using System.Collections.Generic;
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
            var baseUrl = viewPage.Model.SiteConfig.CssPath;
            return viewPage.CSS(url).BasedAt(baseUrl);
        }

        public static ScriptReferenceExpression SkinScript(this IAltOxitePage viewPage, string url)
        {
            var baseUrl = viewPage.Model.SiteConfig.ScriptsPath;
            return viewPage.Script(url).BasedAt(baseUrl);
        }

        public static ScriptReferenceExpression SkinScript(this IAltOxitePage viewPage, IEnumerable<string> urls)
        {
            var baseUrl = viewPage.Model.SiteConfig.ScriptsPath;
            return viewPage.Script(urls).BasedAt(baseUrl);
        }

        public static LoginStatusExpression DisplayLoginStatus(this IAltOxitePage viewPage)
        {
            var renderer = ServiceLocator.Current.GetInstance<IWebFormsViewRenderer>();
            return new LoginStatusExpression(viewPage, renderer);
        }

        public static BlogPostExpression DisplayBlogPost(this IAltOxitePage viewPage)
        {
            var renderer = ServiceLocator.Current.GetInstance<IWebFormsViewRenderer>();
            return new BlogPostExpression(viewPage, renderer);
        }

        public static TagLinkListExpression DisplayTagList(this IAltOxitePage viewPage)
        {
            var renderer = ServiceLocator.Current.GetInstance<IWebFormsViewRenderer>();
            return new TagLinkListExpression(viewPage, renderer);
        }
    }
}