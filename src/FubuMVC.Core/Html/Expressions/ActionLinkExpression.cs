//using System;
//using System.Linq.Expressions;
//using FubuMVC.Core.Controller;
//using FubuMVC.Core.Routing;

//namespace FubuMVC.Core.Html.Expressions
//{
//    public class ActionLinkExpression<CONTROLLER> : HtmlExpressionBase
//        where CONTROLLER : IFubuController
//    {
//        private readonly string _actionUrl;
//        private readonly string _linkText;

//        public ActionLinkExpression(IControllerFactory factory, Expression<Func<CONTROLLER, object>> controllerActionExpression, string linkText)
//        {
//            _linkText = linkText;
//            _actionUrl = factory.UrlFor(controllerActionExpression);
//        }

//        public override string ToString()
//        {
//            return string.Format("<a href=\"{0}\"{1}>{2}</a>",
//                _actionUrl,
//                GetHtmlAttributesString(),
//                _linkText);
//        }
//    }
//}