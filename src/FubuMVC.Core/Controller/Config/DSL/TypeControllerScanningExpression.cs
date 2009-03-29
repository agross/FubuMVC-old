using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FubuMVC.Core.Controller.Config.DSL
{
    public interface ITypeScanningExpression
    {
        IMethodScanningExpression SelectTypes(Func<Type, bool> typeSelector);
    }

    public interface IMethodScanningExpression
    {
        void SelectMethods(Func<MethodInfo, bool> methodSelector);
    }

    public class TypeControllerScanningExpression : ITypeScanningExpression, IMethodScanningExpression
    {
        private const BindingFlags _methodFlags = BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance;

        private AssemblyControllerScanningExpression _assemblyExpression;
        private readonly IEnumerable<Type> _initialTypes;


        public TypeControllerScanningExpression(AssemblyControllerScanningExpression expression, IEnumerable<Type> initialTypeList)
        {
            _assemblyExpression = expression;
            _initialTypes = initialTypeList;
        }

        public IEnumerable<Type> DiscoveredTypes { get; private set; }
        public IEnumerable<MethodInfo> DiscoveredActions { get; private set; }

        public IMethodScanningExpression SelectTypes(Func<Type, bool> typeSelector)
        {
            DiscoveredTypes = _initialTypes.Where(typeSelector);
            SelectMethods(m => true);
            return this;
        }

        public void SelectMethods(Func<MethodInfo, bool> methodSelector)
        {
            DiscoveredActions =
                from t in DiscoveredTypes
                from m in t.GetMethods(_methodFlags)
                where methodSelector(m)
                select m;
        }
    }
}