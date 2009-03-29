using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FubuMVC.Core.Controller.Config.DSL
{
    public interface IAssemblyControllerScanningExpression
    {
        void UsingTypesInTheSameAssemblyAs<T>(Func<IEnumerable<Type>, IEnumerable<MethodInfo>> typeScanner);
        void UsingTypesInTheSameAssemblyAs<T>(Action<ITypeScanningExpression> expressionAction);
        void UsingConfigurer(IControllerActionConfigurer configurer);
    }

    public class AssemblyControllerScanningExpression : IAssemblyControllerScanningExpression
    {
        private readonly IEnumerable<IControllerActionConfigurer> _standardConfigurers;
        private readonly IList<IControllerActionConfigurer> _customConfigurers = new List<IControllerActionConfigurer>();
        private readonly IList<ControllerActionConfig> _actionConfigs = new List<ControllerActionConfig>();
        private readonly FubuConventions _conventions;

        public AssemblyControllerScanningExpression(FubuConventions conventions, IEnumerable<IControllerActionConfigurer> standardConfigurers)
        {
            _conventions = conventions;
            _standardConfigurers = standardConfigurers;
        }

        public IEnumerable<IControllerActionConfigurer> CustomConfigurers { get { return _customConfigurers; } }

        public IEnumerable<ControllerActionConfig> DiscoveredConfigs
        {
            get { return _actionConfigs.AsEnumerable(); }
        }

        public void UsingTypesInTheSameAssemblyAs<T>(Action<ITypeScanningExpression> expressionAction)
        {
            var expression = new TypeControllerScanningExpression(this, GetPotentialControllerTypes<T>());

            expressionAction(expression);

            AddDiscoveredActions(expression.DiscoveredActions);
        }

        public void UsingTypesInTheSameAssemblyAs<T>(Func<IEnumerable<Type>, IEnumerable<MethodInfo>> typeScanner)
        {
            var initialTypeList = GetPotentialControllerTypes<T>();

            var methods = typeScanner(initialTypeList);

            AddDiscoveredActions(methods);
        }

        public IEnumerable<Type> GetPotentialControllerTypes<T>()
        {
            var assembly = typeof(T).Assembly;
            return assembly.GetExportedTypes().Where(
                type =>
                    !type.IsAbstract
                    && !type.IsValueType);
        }

        public void UsingConfigurer(IControllerActionConfigurer configurer)
        {
            _customConfigurers.Add(configurer);
        }

        public void AddDiscoveredActions(IEnumerable<MethodInfo> methods)
        {
            _actionConfigs.AddRange(methods.Select(method => AddConfigFromDiscoveredAction(method)));
        }

        public ControllerActionConfig AddConfigFromDiscoveredAction(MethodInfo method)
        {
            var configurer = _customConfigurers.FirstOrDefault(c => c.ShouldConfigure(method));

            if (configurer == null) configurer = _standardConfigurers.FirstOrDefault(c => c.ShouldConfigure(method));

            if( configurer == null )
            {
                throw new InvalidOperationException(
                    "None of the controller action configurers indicated that they could configure the method '{0}' on controller '{1}'. Either filter these types of methods out of the list of potential action methods during configuration, or create a custom configurer that can configure them as actions."
                        .ToFormat(method.Name, method.DeclaringType.Name));
            }


            var config = configurer.Configure(method);
            config.PrimaryUrl = _conventions.PrimaryUrlConvention(config);

            return config;
        }
    }
}