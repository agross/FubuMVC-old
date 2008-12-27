using System;

namespace FubuMVC.Core.Controller.Config.DSL
{
    public class AssemblyControllerScanningExpression
    {
        private readonly AutoControllerConfiguration _autoConfig;
        private readonly FubuConfiguration _fubuConfig;
        private readonly FubuConventions _conventions;

        public AssemblyControllerScanningExpression(FubuConfiguration fubuConfig, FubuConventions conventions)
        {
            _fubuConfig = fubuConfig;
            _conventions = conventions;
            _autoConfig = new AutoControllerConfiguration();
        }

        public void ContainingType<T>(Action<ControllerTypeScanningExpression> typeScannerAction)
        {
            var assembly = typeof (T).Assembly;
            var expression = new ControllerTypeScanningExpression(_autoConfig, assembly);
            typeScannerAction(expression);

            apply_autoconfig_to_fubuconfig();
        }

        private void apply_autoconfig_to_fubuconfig()
        {
            _autoConfig.GetDiscoveredActions().Each(action =>
            {
                AddConfigFromDiscoveredAction(action);
            });
        }

        public ControllerActionConfig AddConfigFromDiscoveredAction(DiscovererdAction action)
        {
            var configurerType = typeof (MethodInfoActionConfigurer<,,>)
                .MakeGenericType(action.ControllerType, action.InputType, action.OutputType);

            var configurer = (IControllerActionConfigurer) Activator.CreateInstance(configurerType);

            return configurer.Configure(action.Action, _fubuConfig, _conventions);
        }

        //TODO: Future?
        //public ControllerTypeScanningExpression Named(string assemblyName)
        //{
        //    throw new NotImplementedException();
        //}

        //TODO: Future?
        //public ControllerTypeScanningExpression CurrentlyExecuting()
        //{
        //    throw new NotImplementedException();
        //}
    }
}