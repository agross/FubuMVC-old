using System;
using System.Collections.Generic;
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
        private readonly IEnumerable<IControllerActionConfigurer> _standardConfigurers;
        private IEnumerable<IFubuConvention<ControllerActionConfig>> _actionConventions;

        public ControllerActionDSL(FubuConfiguration config, FubuConventions conventions)
            : this( config, conventions, new[]
                {
                    new ThunderdomeActionConfigurer()
                })
        {
        }

        public ControllerActionDSL(FubuConfiguration config, FubuConventions conventions, IEnumerable<IControllerActionConfigurer> standardConfigurers)
        {
            _conventions = conventions;
            _config = config;
            _standardConfigurers = standardConfigurers;
        }

        public FubuConventions Conventions { get { return _conventions; } }

        public ByDefaultDSLChain ByDefault { get { return new ByDefaultDSLChain(_config); } }

        public void UsingConventions(Action<FubuConventions> conventionFunc)
        {
            conventionFunc(_conventions);
        }

        public void ActionConventions(Action<ActionConventionExpression> conventionAction)
        {
            var convExpression = new ActionConventionExpression(_conventions);
            conventionAction(convExpression);
            _actionConventions = convExpression.Conventions;
        }

        public void UsingCustomConventionsFor<TARGET>(Action<CustomConventionExpression<TARGET>> conventionAction)
            where TARGET : class
        {
            conventionAction(new CustomConventionExpression<TARGET>(_conventions));
        }

        public void AddControllerActions(Action<IAssemblyControllerScanningExpression> expressionAction)
        {
            var expression = new AssemblyControllerScanningExpression(_conventions, _standardConfigurers);
            expressionAction(expression);
            
            expression.DiscoveredConfigs.Each(actionConfig =>
            {
                _actionConventions.Each(c => c.Apply(actionConfig));
                _config.AddControllerActionConfig(actionConfig);
            });
        }

        public void OverrideConfigFor<CONTROLLER>(Expression<Func<CONTROLLER, object>> expression, Action<ControllerActionConfig> configAction)
            where CONTROLLER : class
        {
            configAction(_config.GetConfigForAction(expression));
        }
    }
}