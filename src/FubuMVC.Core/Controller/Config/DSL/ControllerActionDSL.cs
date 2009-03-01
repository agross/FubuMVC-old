using System;
using System.Linq.Expressions;
using FubuMVC.Core.Conventions;

namespace FubuMVC.Core.Controller.Config.DSL
{
    public class ByDefaultDSLChain
    {
        private readonly FubuConfiguration _config;

        public ByDefaultDSLChain(FubuConfiguration configuration)
        {
            _config = configuration;
        }

        public void EveryControllerAction(Action<BehaviorExpression> behaviorExpression)
        {
            behaviorExpression(new BehaviorExpression(_config));
        }
    }

    public class ControllerActionDSL
    {
        private readonly FubuConventions _conventions;
        private readonly FubuConfiguration _config;

        public ControllerActionDSL(FubuConfiguration config, FubuConventions conventions)
        {
            _conventions = conventions;
            _config = config;
        }

        public FubuConventions Conventions { get { return _conventions; } }

        public ByDefaultDSLChain ByDefault { get { return new ByDefaultDSLChain(_config); } }

        [Obsolete("Use ByDefault.EveryControllerAction(...) instead", true)]
        public void ByDefaultActions(Action<BehaviorExpression> behaviorExpression)
        {
            behaviorExpression(new BehaviorExpression(_config));
        }

        public void UsingConventions(Action<FubuConventions> conventionFunc)
        {
            conventionFunc(_conventions);
        }

        public void ActionConventions(Action<CustomConventionExpression<ControllerActionConfig>> conventionAction)
        {
            conventionAction(new CustomConventionExpression<ControllerActionConfig>(_conventions));
        }

        public void UsingCustomConventionsFor<TARGET>(Action<CustomConventionExpression<TARGET>> conventionAction)
            where TARGET : class
        {
            conventionAction(new CustomConventionExpression<TARGET>(_conventions));
        }

        public ControllerActionDSL ForController<CONTROLLER>(
            Action<IConfigureActionExpression<CONTROLLER>> configAction)
            where CONTROLLER : class
        {
            var controllerExpression = new ControllerActionConfigExpression<CONTROLLER>(_config, _conventions);
            configAction(controllerExpression);
            
            return this;
        }

        public AssemblyControllerScanningExpression AddControllersFromAssembly
        {
            get
            {
                return new AssemblyControllerScanningExpression(_config, _conventions);
            }
        }

        public void OverrideConfigFor<CONTROLLER>(Expression<Func<CONTROLLER, object>> expression, Action<ControllerActionConfig> configAction)
            where CONTROLLER : class
        {
            configAction(_config.GetConfigForAction(expression));
        }
    }
}