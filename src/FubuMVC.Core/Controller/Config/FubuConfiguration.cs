using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Conventions;
using FubuMVC.Core.Util;

namespace FubuMVC.Core.Controller.Config
{
    public class FubuConfiguration
    {
        private readonly HashSet<Type> _behaviors = new HashSet<Type>();
        private readonly IList<ControllerActionConfig> _actionConfigs = new List<ControllerActionConfig>();

        private readonly Cache<Type, string> _defaultUrlByController = new Cache<Type, string>();
        private readonly FubuConventions _conventions;

        private readonly IList<IFubuConvention<ControllerActionConfig>> _actionConventions =
            new List<IFubuConvention<ControllerActionConfig>>();

        public FubuConfiguration(FubuConventions conventions)
        {
            _conventions = conventions;
        }

        public void AddDefaultBehavior<BEHAVIOR>()
            where BEHAVIOR : IControllerActionBehavior
        {
            _behaviors.Add(typeof (BEHAVIOR));
        }

        public IEnumerable<Type> GetDefaultBehaviors()
        {
            return _behaviors.AsEnumerable();
        }

        public void AddConvention(IFubuConvention<ControllerActionConfig> convention)
        {
            _actionConventions.Add(convention);
        }

        public IEnumerable<IFubuConvention<ControllerActionConfig>> GetActionConventions()
        {
            return _actionConventions.AsEnumerable();
        }


        public void AddControllerActionConfig(ControllerActionConfig config)
        {
            if( _actionConfigs.Exists(cfg=>cfg.IsTheSameActionAs(config)))
            {
                throw new InvalidOperationException(
                    "An attempt was made to configure the same controller action (action: {0}, controller: {1}) twice.  This action has already been configured."
                    .ToFormat(config.ActionName, config.ControllerType.FullName));
            }

            config.ApplyDefaultBehaviors(GetDefaultBehaviors());
            _actionConventions.Each(c => c.Apply(config));
            _actionConfigs.Add(config);

            var defaultPathToController = _conventions.DefaultUrlForController(config.ControllerType);
            _defaultUrlByController.Store(config.ControllerType, defaultPathToController);
        }

        public IEnumerable<ControllerActionConfig> GetControllerActionConfigs()
        {
            return _actionConfigs.AsEnumerable();
        }

        public string GetDefaultUrlFor<CONTROLLER>()
            where CONTROLLER : class
        {
            return GetDefaultUrlFor(typeof(CONTROLLER));
        }

        public string GetDefaultUrlFor(Type controllerType)
        {
            return _defaultUrlByController.Retrieve(controllerType);
        }

        public string GetDefaultUrlFor(ControllerActionConfig config)
        {
            return config.PrimaryUrl;
        }

        public string GetDefaultUrlFor<CONTROLLER>(Expression<Func<CONTROLLER, object>> actionExpression)
            where CONTROLLER : class
        {
            var config = GetConfigForAction(actionExpression);
            return GetDefaultUrlFor(config);
        }

        public ControllerActionConfig GetConfigForAction<CONTROLLER>(Expression<Func<CONTROLLER, object>> actionExpression)
            where CONTROLLER : class
        {
            return GetControllerActionConfigs().FirstOrDefault(c => c.IsTheSameActionAs(actionExpression));
        }
    }
}