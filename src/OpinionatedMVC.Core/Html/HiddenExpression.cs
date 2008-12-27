using System;
using System.Linq.Expressions;
using OpinionatedMVC.Core.Util;

namespace OpinionatedMVC.Core.Html
{
    public class HiddenExpression<VIEWMODEL> : BoundExpression<VIEWMODEL, HiddenExpression<VIEWMODEL>> where VIEWMODEL : class
    {
        public static HiddenExpression<VIEWMODEL> Build(VIEWMODEL model, Expression<Func<VIEWMODEL, object>> expression, string prefix)
        {
            Accessor accessor = ReflectionHelper.GetAccessor(expression);
            object rawValue = accessor.GetValue(model);

            return new HiddenExpression<VIEWMODEL>(accessor.Name, prefix, rawValue);
        }

        public HiddenExpression(VIEWMODEL model, Expression<Func<VIEWMODEL, object>> expression, string prefix)
            : base(model, expression, prefix)
        {
        }

        public HiddenExpression(string name, string prefix, object rawValue) : base(name, prefix, rawValue)
        {
        }

        public override string ToString()
        {
            return string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\"{2} />", name, rawValue ?? string.Empty, GetHtmlAttributesString());
        }

        protected override HiddenExpression<VIEWMODEL> thisInstance()
        {
            return this;
        }
    }
}