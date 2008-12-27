using System;
using System.Linq.Expressions;
using OpinionatedMVC.Core.Controller;

namespace OpinionatedMVC.Core.Html
{
    public class FormExpression<CONTROLLER> : FormExpression 
        where CONTROLLER : IOpinionatedController
    {
        public FormExpression(IUrlProvider registry, Expression<Func<CONTROLLER, object>> expression, params object[] others)
            : base(registry.UrlFor(expression))
        {
        }
    }
    
    public class FormExpression : ExpressionBase
    {
        public FormExpression(string action)
        {
            Action = action;
            DefaultId = "mainForm";
        }

        protected string Action { get; set; }
        protected object[] Others { get; set; }

        public FormExpression WithIdParam(long id)
        {
            Action += "/" + id;
            return this;
        }

        public override string ToString()
        {
            string html = string.Format("<form action=\"{0}\" method=\"post\"{1}>", Action, GetHtmlAttributesString());
            return html;
        }
    }
}
