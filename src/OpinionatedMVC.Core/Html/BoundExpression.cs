using System;
using System.Linq.Expressions;
using OpinionatedMVC.Core.Util;

namespace OpinionatedMVC.Core.Html
{
    public abstract class BoundExpression<VIEWMODEL, THIS> : ExpressionBase where VIEWMODEL : class
    {
        private readonly string _name;
        private readonly string _prefix;
        private readonly object _rawValue;
        protected string validators;
        protected int maxStringLength;

        protected BoundExpression(string name, string prefix, object rawValue)
        {
            _name = name;
            _prefix = prefix;
            _rawValue = rawValue;
        }

        protected BoundExpression(VIEWMODEL model, Expression<Func<VIEWMODEL, object>> expression, string prefix)
        {
            _prefix = prefix;

            Accessor accessor = ReflectionHelper.GetAccessor(expression);
            if (model != null)
            {
                // Naive for the moment
                _rawValue = accessor.GetValue(model);
            }

            addValidation(accessor);

            _name = _prefix + accessor.Name;
        }

        private void addValidation(Accessor accessor)
        {
            //TODO: This should be conventional somehow
            //accessor.ForAttribute<RequiredAttribute>(att =>
            //{
            //    respondToRequireAttribute(att);
            //});

            //accessor.ForAttribute<MaximumStringLengthAttribute>(att =>
            //{
            //    respondToMaxLengthAttribute(att);
            //});
        }

        //TODO: These should be a convention
        //protected virtual void respondToMaxLengthAttribute(MaximumStringLengthAttribute maximumStringLengthAttribute)
        //{
        //}

        //protected virtual void respondToRequireAttribute(RequiredAttribute requiredAttribute)
        //{
        //    CssClasses.Add("required");
        //}

        protected object rawValue
        {
            get { return _rawValue; }
        }

        protected string name
        {
            get { return _name; }
        }

        protected virtual object getValue()
        {
            return rawValue;
        }

        protected abstract THIS thisInstance();

        public THIS AutoQueryFor(Expression<Func<VIEWMODEL, object>> expression)
        {
            string name = ReflectionHelper.GetAccessor(expression).Name;
            string cssClass = name + "AutoQuery";
            CssClasses.Add(cssClass);

            return thisInstance();
        }
    }
}