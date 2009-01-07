
using System;
using System.Linq.Expressions;

namespace FubuMVC.Core.Html.Expressions
{
    public class WhenExpression
    {
        public WhenExpression<MODEL> For<MODEL>(MODEL model)
            where MODEL : class
        {
            return new WhenExpression<MODEL>(model);
        }
    }

    public class WhenExpression<MODEL>
        where MODEL : class
    {
        private MODEL _model;
        private Func<MODEL, bool> _validator;

        public WhenExpression(MODEL model)
        {
            _model = model;
        }

        public WhenExpression<MODEL> When(Func<MODEL, bool> validator)
        {
            _validator = validator;
            return this;
        }

        public string Display(string text)
        {
            return (_validator.Invoke(_model)) ? text : "";
        }
    }
}