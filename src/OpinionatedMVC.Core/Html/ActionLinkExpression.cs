using System;
using System.Linq.Expressions;
using OpinionatedMVC.Core.Controller;

namespace OpinionatedMVC.Core.Html
{
	public class ActionLinkExpression<CONTROLLER> : ExpressionBase
        where CONTROLLER : IOpinionatedController
	{
		private readonly string _actionUrl;
		private readonly string _linkText;

		public ActionLinkExpression(IUrlProvider provider, Expression<Func<CONTROLLER, object>> controllerActionExpression, string linkText)
		{
            _linkText = linkText;
            _actionUrl = provider.UrlFor(controllerActionExpression);
		}

		public override string ToString()
		{
			return string.Format("<a href=\"{0}\"{1}>{2}</a>",
				_actionUrl,
				GetHtmlAttributesString(),
				_linkText);
		}
	}
}