using System;
using System.Linq.Expressions;
using System.Reflection;
using FubuMVC.Core.Controller.Invokers;
using FubuMVC.Core.Util;

namespace FubuMVC.Core.Controller.Config.DSL
{
    public class RedirectActionConfigurer : IControllerActionConfigurer
    {
        private static readonly Type _invokerOpenType = typeof (RedirectActionInvoker<,>);
        private static readonly Type _invokerInputType = _invokerOpenType.GetGenericArguments()[1];

        public bool ShouldConfigure(MethodInfo method)
        {
            if (!method.IsPublic) return false;
            if (method.IsGenericMethod) return false;

            var methodParams = method.GetParameters();
            if (methodParams.Length != 1) return false;

            var argType = method.GetParameters()[0].ParameterType;
            var returnType = method.ReturnType;

            if (!ReflectionHelper.MeetsSpecialGenericConstraints(_invokerInputType, argType)) return false;

            if (!argType.IsClass || returnType != typeof(void)) return false;

            return true;
        }

        public ControllerActionConfig Configure(MethodInfo method)
        {
            var inputType = method.GetParameters()[0].ParameterType;
            var invokerType = typeof (RedirectActionInvoker<,>)
                .MakeGenericType(method.DeclaringType, inputType);

            var methodDelegate = OMIZMOExpressionFrom(method);
            var config = new ControllerActionConfig(method, invokerType, methodDelegate);

            return config;
        }

        public Delegate OMIZMOExpressionFrom(MethodInfo method)
        {
            // Produces a lambda like:  (c,i) => c.YourMethod(i)
            var controllerType = method.DeclaringType;
            var inputType = method.GetParameters()[0].ParameterType;

            var controllerParam = Expression.Parameter(controllerType, "c");
            var inputParam = Expression.Parameter(inputType, "i");

            var methodCall = Expression.Call(controllerParam, method, inputParam);

            var lambda = Expression.Lambda(methodCall, controllerParam, inputParam);

            return lambda.Compile();
        }

    }
}