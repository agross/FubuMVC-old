using System;
using System.Linq.Expressions;
using FubuMVC.Core.Util;

namespace FubuMVC.Core.Html.Expressions
{
    public abstract class BoundExpression<VIEWMODEL, THIS> : HtmlExpressionBase 
        where VIEWMODEL : class
    {
        private readonly string _name;
        private readonly string _prefix;
        private readonly object _rawValue;
        protected string validators;
        protected int maxStringLength;
        protected Accessor expressionAccessor;

        protected BoundExpression(string name, string prefix, object rawValue)
        {
            _name = name;
            _prefix = prefix;
            _rawValue = rawValue;
        }

        protected BoundExpression(VIEWMODEL model, Expression<Func<VIEWMODEL, object>> expression, string prefix)
        {
            _prefix = prefix;

            expressionAccessor = ReflectionHelper.GetAccessor(expression);
            if (model != null)
            {
                // Naive for the moment
                _rawValue = expressionAccessor.GetValue(model);
            }

            _name = _prefix + expressionAccessor.Name;
        }


        protected object rawValue
        {
            get { return _rawValue; }
        }

        public string Name
        {
            get { return _name; }
        }

        protected virtual object getValue()
        {
            return rawValue;
        }

        protected abstract THIS thisInstance();
    }
}