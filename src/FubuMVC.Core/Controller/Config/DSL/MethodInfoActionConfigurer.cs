using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FubuMVC.Core.Controller.Config.DSL
{
    public interface IControllerActionConfigurer
    {
        ControllerActionConfig Configure(MethodInfo method, FubuConfiguration fubuConfig, FubuConventions conventions);
    }

    public class MethodInfoActionConfigurer<CONTROLLER, INPUT, OUTPUT> : IControllerActionConfigurer
        where CONTROLLER : class
        where INPUT : class, new()
        where OUTPUT : class
    {
        public ControllerActionConfig Configure(MethodInfo method, FubuConfiguration fubuConfig, FubuConventions conventions)
        {
            var expression = ExpressionFrom(method);
            return ControllerActionConfigExpression<CONTROLLER>.AddActionToConfig(fubuConfig, conventions, expression);
        }

        public Expression<Func<CONTROLLER, INPUT, OUTPUT>> ExpressionFrom(MethodInfo method)
        {
            // Produces a lambda like:  (c,i) => c.YourMethod(i)

            var controllerParam = Expression.Parameter(typeof (CONTROLLER), "c");
            var inputParam = Expression.Parameter(typeof (INPUT), "i");
            
            var methodCall = Expression.Call(controllerParam, method, inputParam);

            var lambda = Expression.Lambda(methodCall, controllerParam, inputParam);

            return (Expression<Func<CONTROLLER, INPUT, OUTPUT>>) lambda;
        }
    }
}