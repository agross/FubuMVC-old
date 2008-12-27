using System;
using System.Reflection;

namespace FubuMVC.Core.Controller.Config.DSL
{
    public class ControllerTypeScanningExpression
    {
        private readonly AutoControllerConfiguration _autoConfig;

        public ControllerTypeScanningExpression(AutoControllerConfiguration autoConfig, Assembly assembly)
        {
            _autoConfig = autoConfig;
            Assembly = assembly;
        }

        public Assembly Assembly{ get; private set;}

        public void Where(Func<Type, bool> evalTypeFunc)
        {
            Assembly.GetExportedTypes().Each(type =>
            {
                if (type.IsAbstract) return;

                if (type.IsValueType) return;

                if (evalTypeFunc(type)) _autoConfig.AddDiscoveredType(type);
            });
        }

        public void MapActionsWhere(Func<MethodInfo, Type, Type, bool> evalMethodFunc)
        {
            _autoConfig.GetDiscoveredTypes().Each(type => 
                type.GetMethods().Each(method =>
                {
                    if (method.IsGenericMethod) return;
                    if (method.ReturnType.Equals(typeof(void))) return;

                    var methodParams = method.GetParameters();
                    if (methodParams.Length != 1) return;

                    var argType = method.GetParameters()[0].ParameterType;
                    var returnType = method.ReturnType;
                    if (!argType.IsClass || !returnType.IsClass) return;

                    if( evalMethodFunc(method, argType, returnType))
                    {
                        var action = new DiscovererdAction
                            {
                                ControllerType = type,
                                Action = method,
                                InputType = argType,
                                OutputType = returnType
                            };
                        _autoConfig.AddDiscoveredAction(action);
                    }
                }));
        }
    }
}