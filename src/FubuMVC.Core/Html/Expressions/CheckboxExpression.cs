using System;
using System.Linq.Expressions;

namespace FubuMVC.Core.Html.Expressions
{
    public class CheckboxExpression<VIEWMODEL> : BoundExpression<VIEWMODEL, CheckboxExpression<VIEWMODEL>> 
        where VIEWMODEL : class
    {
        private readonly bool _isChecked;

        public CheckboxExpression(VIEWMODEL viewModel, Expression<Func<VIEWMODEL, object>> expression, string prefix)
            : base(viewModel, expression, prefix)
        {
            _isChecked = (bool)rawValue;
        }

        public override string ToString()
        {
            const string template = "<input type=\"checkbox\" value=\"{0}\" name=\"{0}\"{1}{2}/>";
            string checkedText = _isChecked ? " checked=\"checked\"" : "";

            return string.Format(template, Name, checkedText, GetHtmlAttributesString());
        }

        protected override CheckboxExpression<VIEWMODEL> thisInstance()
        {
            return this;
        }
    }
}