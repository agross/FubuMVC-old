using System;
using System.Linq.Expressions;
using FubuMVC.Core.Util;

namespace FubuMVC.Core.Html.Expressions
{
    public class TextBoxExpression<VIEWMODEL> : BoundExpression<VIEWMODEL, TextBoxExpression<VIEWMODEL>>
        where VIEWMODEL : class
    {
        private readonly Accessor _accessor;
        private readonly object _value;
        private bool _multiLine;
        private bool _password;
        private readonly string _name;

        public TextBoxExpression(VIEWMODEL viewModel, Expression<Func<VIEWMODEL, object>> expression, string prefix)
            : base(viewModel, expression, prefix)
        {
            _accessor = ReflectionHelper.GetAccessor(expression);
            _name = prefix + _accessor.Name;
            
            if(!Equals(viewModel, default(VIEWMODEL)))
            {
                _value = _accessor.GetValue(viewModel);
            }
        }

        public TextBoxExpression<VIEWMODEL> MultilineMode()
        {
            _multiLine = true;
            return this;
        }

        public TextBoxExpression<VIEWMODEL> PasswordMode()
        {
            _password = true;
            return this;
        }

        public override string ToString()
        {
            string htmlFormat = _multiLine
                                ? "<textarea name=\"{1}\"{3} >{2}</textarea>"
                                : "<input type=\"{0}\" name=\"{1}\" value=\"{2}\"{3} />";

            string inputType = _password ? "password" : "text";

            return string.Format(htmlFormat, inputType, _name, _value, GetHtmlAttributesString());
        }

        protected override TextBoxExpression<VIEWMODEL> thisInstance()
        {
            return this;
        }
    }
}