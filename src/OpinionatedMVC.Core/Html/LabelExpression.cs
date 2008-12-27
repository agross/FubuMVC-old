using System;
using System.Linq.Expressions;

namespace OpinionatedMVC.Core.Html
{
    public class LabelExpression<VIEWMODEL> : BoundExpression<VIEWMODEL, LabelExpression<VIEWMODEL>> where VIEWMODEL : class
    {
        private string _valueFormat = "{0}";

        public LabelExpression(VIEWMODEL viewModel, Expression<Func<VIEWMODEL, object>> expression, string prefix)
            : base(viewModel, expression, prefix)
        {
        }

        public LabelExpression<VIEWMODEL> Format(string format)
        {
            _valueFormat = format;

            return this;
        }

        public override string ToString()
        {
            var value = getValue();

            var valueAsString = value as string;

            if (value != null && (valueAsString == null || valueAsString.IsNotEmpty()) )
            {
                value = string.Format(_valueFormat, value);
            }

            return string.Format("<span name=\"{0}\"{1}>{2}</span>", name, GetHtmlAttributesString(), value);
        }

        protected override LabelExpression<VIEWMODEL> thisInstance()
        {
            return this;
        }
    }
}