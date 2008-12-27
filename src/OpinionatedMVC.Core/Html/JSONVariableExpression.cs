using System;
using System.Linq.Expressions;
using OpinionatedMVC.Core.Util;

namespace OpinionatedMVC.Core.Html
{
    public class VariableExpression<VIEWMODEL> : BoundExpression<VIEWMODEL, VariableExpression<VIEWMODEL>> where VIEWMODEL : class
    {
        private readonly string _variable;

        public VariableExpression(string variable, VIEWMODEL model, Expression<Func<VIEWMODEL, object>> expression, string prefix)
            : base(model, expression, prefix)
        {
            _variable = variable;
        }

        public override string ToString()
        {
            string format = rawValue is string ? "var {0} = '{1}';" : "var {0} = {1};";
            return string.Format(format, _variable, rawValue);
        }

        protected override VariableExpression<VIEWMODEL> thisInstance()
        {
            return this;
        }
    }

    public class JSONVariableExpression<VIEWMODEL> : BoundExpression<VIEWMODEL, JSONVariableExpression<VIEWMODEL>>
        where VIEWMODEL : class
    {
        private readonly string _variable;

        public JSONVariableExpression(string variable, VIEWMODEL model, Expression<Func<VIEWMODEL, object>> expression, string prefix)
            : base(model, expression, prefix)
        {
            _variable = variable;
        }

        public JSONVariableExpression(string variable, VIEWMODEL model, object value, string prefix)
            : base("", prefix, value)
        {
            _variable = variable;
        }

        public override string ToString()
        {
            return string.Format("var {0} = {1};", _variable, JsonUtil.ToJson(rawValue));
        }

        protected override JSONVariableExpression<VIEWMODEL> thisInstance()
        {
            return this;
        }
    }
}
