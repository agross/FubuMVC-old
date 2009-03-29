using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FubuMVC.Core.Controller.Config.DSL
{
    public class ThunderdomeActionConfigurer : IControllerActionConfigurer
    {
        public bool ShouldConfigure(MethodInfo method)
        {
            if (!method.IsPublic) return false;
            if (method.IsGenericMethod) return false;
            if (method.ReturnType.Equals(typeof(void))) return false;

            var methodParams = method.GetParameters();
            if (methodParams.Length != 1) return false;

            var argType = method.GetParameters()[0].ParameterType;
            var returnType = method.ReturnType;
            if (!argType.IsClass || !returnType.IsClass) return false;

            return true;
        }

        public ControllerActionConfig Configure(MethodInfo method)
        {
            var inputType = method.GetParameters()[0].ParameterType;
            var outputType = method.ReturnType;
            var invokerType = typeof (ThunderdomeActionInvoker<,,>)
                .MakeGenericType(method.DeclaringType, inputType, outputType);

            var methodDelegate = OMIOMOExpressionFrom(method);
            var config = new ControllerActionConfig(method, invokerType, methodDelegate);

            return config;
        }

        public Delegate OMIOMOExpressionFrom(MethodInfo method)
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