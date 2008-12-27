using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.Practices.ServiceLocation;
using OpinionatedMVC.Core.Controller;
using OpinionatedMVC.Core.Util;
using OpinionatedMVC.Core.View;

namespace OpinionatedMVC.Core.Html
{
    public static class ViewHtmlExpressionExtensions
    {
        public static string Content(this IView viewPage, string url)
        {
            // TODO: return url.Replace("~", UrlProvider.BaseUrl);
            return "";
        }

        public static string CSS(this IView viewPage, string url)
        {
            var cacheSet = ServiceLocator.Current.GetInstance<ICachedSet>();
            var baseFolder = Content(viewPage, "~/Content/Styles/");
            const string template = @"<link href=""{0}{1}"" rel=""stylesheet"" type=""text/css"" />";
            return new DistinctFileIncludeExpression(cacheSet, template, baseFolder, url).ToString();

        }

        public static string Script(this IView viewPage, string url)
        {
            var cacheSet = ServiceLocator.Current.GetInstance<ICachedSet>();
            string baseFolder = Content(viewPage, "~/Content/Scripts/");
            const string template = @"<script type=""text/javascript"" src=""{0}{1}""></script>";
            return new DistinctFileIncludeExpression(cacheSet, template, baseFolder, url).ToString();
        }

        public static RenderPartialExpression<VIEWMODEL> RenderPartialForEachOf<VIEWMODEL, PARTIALVIEWMODEL>(
            this IViewWithModel<VIEWMODEL> viewPage, Expression<Func<VIEWMODEL, IEnumerable<PARTIALVIEWMODEL>>> expression)
            where VIEWMODEL : class
            where PARTIALVIEWMODEL : class
        {
            var renderer = ServiceLocator.Current.GetInstance<IPartialRenderer>();
            return new RenderPartialExpression<VIEWMODEL>(viewPage, renderer).ForEachOf(expression);
        }

        public static RenderPartialExpression<VIEWMODEL> RenderPartialFor<VIEWMODEL, PARTIALVIEWMODEL>(
            this IViewWithModel<VIEWMODEL> viewPage, Expression<Func<VIEWMODEL, PARTIALVIEWMODEL>> expression)
            where VIEWMODEL : class
            where PARTIALVIEWMODEL : class
        {
            var renderer = ServiceLocator.Current.GetInstance<IPartialRenderer>();
            return new RenderPartialExpression<VIEWMODEL>(viewPage, renderer).For(expression);
        }

        public static RenderPartialExpression<VIEWMODEL> RenderPartial<VIEWMODEL>(this IViewWithModel<VIEWMODEL> viewPage) where VIEWMODEL : class
        {
            var renderer = ServiceLocator.Current.GetInstance<IPartialRenderer>();
            return new RenderPartialExpression<VIEWMODEL>(viewPage, renderer).For(viewPage.ViewModel);
        }

        public static DropDownListExpression<VIEWMODEL> DropDownListFor<VIEWMODEL>(
            this IViewWithModel<VIEWMODEL> viewPage, Expression<Func<VIEWMODEL, object>> expression)
            where VIEWMODEL : class
        {
            return new DropDownListExpression<VIEWMODEL>(viewPage.ViewModel, expression, viewPage.NamePrefix);
        }

        public static CheckboxExpression<VIEWMODEL> CheckBoxFor<VIEWMODEL>(
            this IViewWithModel<VIEWMODEL> viewPage, Expression<Func<VIEWMODEL, object>> expression)
            where VIEWMODEL : class
        {
            return new CheckboxExpression<VIEWMODEL>(viewPage.ViewModel, expression, viewPage.NamePrefix);
        }

        public static TextBoxExpression<VIEWMODEL> TextBoxFor<VIEWMODEL>(
            this IViewWithModel<VIEWMODEL> viewPage, Expression<Func<VIEWMODEL, object>> expression)
            where VIEWMODEL : class
        {
            return new TextBoxExpression<VIEWMODEL>(viewPage.NamePrefix, viewPage.ViewModel, expression);
        }

        public static TextBoxExpression<VIEWMODEL> TextAreaFor<VIEWMODEL>(
            this IViewWithModel<VIEWMODEL> viewPage, Expression<Func<VIEWMODEL, object>> expression)
            where VIEWMODEL : class
        {
            return new TextBoxExpression<VIEWMODEL>(viewPage.NamePrefix, viewPage.ViewModel, expression).MultilineMode();
        }

        public static LabelExpression<VIEWMODEL> LabelFor<VIEWMODEL>(
            this IViewWithModel<VIEWMODEL> viewPage, Expression<Func<VIEWMODEL, object>> expression)
            where VIEWMODEL : class
        {
            return new LabelExpression<VIEWMODEL>(viewPage.ViewModel, expression, viewPage.NamePrefix);
        }

        public static HiddenExpression<VIEWMODEL> HiddenFor<VIEWMODEL>(this IViewWithModel<VIEWMODEL> viewPage,
                                                                       Expression<Func<VIEWMODEL, object>> expression)
            where VIEWMODEL : class
        {
            return new HiddenExpression<VIEWMODEL>(viewPage.ViewModel, expression, viewPage.NamePrefix);
        }

        public static TextBoxExpression<VIEWMODEL> PasswordFor<VIEWMODEL>(this IViewWithModel<VIEWMODEL> viewPage,
                                                    Expression<Func<VIEWMODEL, object>> expression)
            where VIEWMODEL : class
        {
            return new TextBoxExpression<VIEWMODEL>(viewPage.NamePrefix, viewPage.ViewModel, expression).PasswordMode();
        }


        public static string ButtonFor(this IView view, string name, string displayName)
        {
            return string.Format(
                "<input type=\"button\" value=\"{0}\" id=\"{1}\" name=\"{1}\" />",
                displayName, name);
        }

        public static string ButtonFor(this IView view, string name, string displayName, string id)
        {
            return string.Format(
                "<input type=\"button\" value=\"{0}\" id=\"{1}\" name=\"{2}\" />",
                displayName, id, name);
        }

        public static ActionLinkExpression<CONTROLLER> LinkToAction<CONTROLLER>(this IView viewPage, Expression<Func<CONTROLLER, object>> actionExpression, string linkText)
            where CONTROLLER : IOpinionatedController
        {
            var urlProvider = ServiceLocator.Current.GetInstance<IUrlProvider>();
            return new ActionLinkExpression<CONTROLLER>(urlProvider, actionExpression, linkText);
        }

        public static string ActionUrl<CONTROLLER>(this IView viewPage, Expression<Func<CONTROLLER, object>> actionExpression)
            where CONTROLLER : IOpinionatedController
        {
            var urlProvider = ServiceLocator.Current.GetInstance<IUrlProvider>();
            return urlProvider.UrlFor(actionExpression);
        }

        public static string ActionUrl<CONTROLLER, VIEW_MODEL>(this IView viewPage, Expression<Func<CONTROLLER, Func<VIEW_MODEL, object>>> expression, object id)
            where CONTROLLER : IOpinionatedController
        {
            var urlProvider = ServiceLocator.Current.GetInstance<IUrlProvider>();
            return urlProvider.UrlForAction(expression, id);
        }

        public static string JsonVariableFor<VIEWMODEL>(this IViewWithModel<VIEWMODEL> viewPage, string variable,
                                                       Expression<Func<VIEWMODEL, object>> expression) where VIEWMODEL : class
        {
            return new JSONVariableExpression<VIEWMODEL>(variable, viewPage.ViewModel, expression, viewPage.NamePrefix).ToString();
        }

        public static string JsonVariableFor<VIEWMODEL>(this IViewWithModel<VIEWMODEL> viewPage, string variable, object data) where VIEWMODEL : class
        {
            return string.Format("var {0} = {1};", variable, JsonUtil.ToJson(data));
        }

        public static string JsonVariableForModel<VIEWMODEL>(this IViewWithModel<VIEWMODEL> viewPage, string variable) where VIEWMODEL : class
        {
            return new JSONVariableExpression<VIEWMODEL>(variable, viewPage.ViewModel, viewPage.ViewModel, viewPage.NamePrefix).ToString();
        }

        public static string VariableFor<VIEWMODEL>(this IViewWithModel<VIEWMODEL> viewPage, string variable,
                                               Expression<Func<VIEWMODEL, object>> expression) where VIEWMODEL : class
        {
            return new JSONVariableExpression<VIEWMODEL>(variable, viewPage.ViewModel, expression, viewPage.NamePrefix).ToString();
        }
    }
}