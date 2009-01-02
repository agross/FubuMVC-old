using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using Microsoft.Practices.ServiceLocation;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Html.Expressions;
using FubuMVC.Core.View;
using FubuMVC.Core.View.WebForms;

namespace FubuMVC.Core.Html
{
    public static class HtmlExtensions
    {
        public static string Content(this IFubuView viewPage, string url)
        {
            return url.ToFullUrl();
        }

        public static string PageTitle(this IFubuView viewPage, string title)
        {
            return "<title>{0}</title>".ToFormat(HttpUtility.HtmlEncode(title));
        }

        public static MetaExpression MetaTag(this IFubuView viewPage)
        {
            return new MetaExpression();
        }

        public static LinkExpression LinkTag(this IFubuView viewPage)
        {
            return new LinkExpression();
        }

        public static LinkExpression CSS(this IFubuView viewPage, string url)
        {
            return new LinkExpression().Href(url).AsStyleSheet();
        }

        public static ScriptReferenceExpression Script(this IFubuView viewPage, string url)
        {
            return new ScriptReferenceExpression().Add(url);
        }

        public static ScriptReferenceExpression Script(this IFubuView viewPage, IEnumerable<string> scriptLinks)
        {
            var expr = new ScriptReferenceExpression();
            scriptLinks.Each(l => expr.Add(l));
            return expr;
        }

        public static string JsonUrl<CONTROLLER>(this IFubuView viewPage, Expression<Func<CONTROLLER, object>> actionExpression)
            where CONTROLLER : class
        {
            return ActionUrl(viewPage, actionExpression) + ".json";
        }

        public static string ActionUrl<CONTROLLER>(this IFubuView viewPage, Expression<Func<CONTROLLER, object>> actionExpression)
            where CONTROLLER : class
        {
            var resolver = ServiceLocator.Current.GetInstance<IUrlResolver>();

            return resolver.UrlFor(actionExpression);
        }

        public static FormExpression FormFor<CONTROLLER>(this IFubuView view, Expression<Func<CONTROLLER, object>> actionExpression)
            where CONTROLLER : class
        {
            var actionUrl = ActionUrl(view, actionExpression);
            return new FormExpression( actionUrl );
        }

        public static string Localize(this IFubuView view, string key)
        {
            return LocalizationManager.GetTextForKey(key);
        }

        public static HiddenExpression<VIEWMODEL> HiddenFor<VIEWMODEL>(this IFubuView<VIEWMODEL> viewPage,
                                                               Expression<Func<VIEWMODEL, object>> expression)
            where VIEWMODEL : class
        {
            return new HiddenExpression<VIEWMODEL>(viewPage.Model, expression, "");
        }

        public static RenderPartialExpression RenderPartial<VIEWMODEL>(this IFubuView<VIEWMODEL> viewPage)
            where VIEWMODEL : class
        {
            var renderer = ServiceLocator.Current.GetInstance<IWebFormsViewRenderer>();
            var conventions = ServiceLocator.Current.GetInstance<FubuConventions>();
            return new RenderPartialExpression(viewPage, renderer, conventions);
        }

        public static string SubmitButton(this IFubuView viewPage, string value, string name)
        {
            return @"<input type=""submit"" value=""{0}"" name=""{1}""/>".ToFormat(
                value,
                name);
        }

        public static ImageExpression Image(this IFubuView viewPage, string imageSrcUrl)
        {
            return new ImageExpression(imageSrcUrl);
        }

        //public static ActionLinkExpression<CONTROLLER> LinkTo<CONTROLLER>(this IFubuView viewPage, Expression<Func<CONTROLLER, object>> actionExpression, string linkText)
        //    where CONTROLLER : IFubuController
        //{
        //    var resolver = ServiceLocator.Current.GetInstance<IUrlResolver>();
        //    return new ActionLinkExpression<CONTROLLER>(resolver, actionExpression, linkText);
        //}

        public static CheckboxExpression<VIEWMODEL> CheckBoxFor<VIEWMODEL>(
           this IFubuView<VIEWMODEL> viewPage, Expression<Func<VIEWMODEL, object>> expression)
           where VIEWMODEL : class
        {
            return new CheckboxExpression<VIEWMODEL>(viewPage.Model, expression, "");
        }

        public static TextBoxExpression<VIEWMODEL> TextBoxFor<VIEWMODEL>(
           this IFubuView<VIEWMODEL> viewPage, Expression<Func<VIEWMODEL, object>> expression)
           where VIEWMODEL : class
        {
            return new TextBoxExpression<VIEWMODEL>(viewPage.Model, expression, "");
        }

    }
}
