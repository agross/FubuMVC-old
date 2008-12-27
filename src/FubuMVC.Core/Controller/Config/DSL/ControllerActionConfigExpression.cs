using System;
using System.Linq.Expressions;

namespace FubuMVC.Core.Controller.Config.DSL
{
    public interface IConfigureActionExpression<CONTROLLER>
        where CONTROLLER : class
    {
        ControllerActionConfigExpression<CONTROLLER> Action<INPUT, OUTPUT>(
            Expression<Func<CONTROLLER, INPUT, OUTPUT>> actionExpression)
            where INPUT : class, new()
            where OUTPUT : class;

        ControllerActionConfigExpression<CONTROLLER> Action<INPUT, OUTPUT>(
            Expression<Func<CONTROLLER, INPUT, OUTPUT>> actionExpression,
            Action<ActionConfigExpression> actionOptions)
            where INPUT : class, new()
            where OUTPUT : class;
    }

    public class ControllerActionConfigExpression<CONTROLLER> : IConfigureActionExpression<CONTROLLER>
        where CONTROLLER : class
    {
        private readonly FubuConventions _conventions;
        private readonly FubuConfiguration _fubuConfig;

        public ControllerActionConfigExpression(FubuConfiguration configuration, FubuConventions conventions)
        {
            _fubuConfig = configuration;
            _conventions = conventions;
        }

        public ControllerActionConfigExpression<CONTROLLER> Action<INPUT, OUTPUT>(
            Expression<Func<CONTROLLER, INPUT, OUTPUT>> actionExpression)
            where INPUT : class, new()
            where OUTPUT : class
        {
            return Action(actionExpression, x => { });
        }

        public ControllerActionConfigExpression<CONTROLLER> Action<INPUT, OUTPUT>(
            Expression<Func<CONTROLLER, INPUT, OUTPUT>> actionExpression,
            Action<ActionConfigExpression> actionOptions)
            where INPUT : class, new()
            where OUTPUT : class
        {
            var config = _fubuConfig.GetConfigForAction(actionExpression);

            config = config ?? AddActionToConfig(_fubuConfig, _conventions, actionExpression);

            var actionConfigExpression = new ActionConfigExpression(config);

            actionOptions(actionConfigExpression);

            return this;
        }

        public static ControllerActionConfig AddActionToConfig<INPUT, OUTPUT>(FubuConfiguration fubuConfig, FubuConventions conventions, Expression<Func<CONTROLLER, INPUT, OUTPUT>> actionExpression)
            where INPUT : class, new()
            where OUTPUT : class
        {
            var config = ControllerActionConfig.ForAction(actionExpression);

            config.PrimaryUrl = conventions.PrimaryUrlConvention(config);

            fubuConfig.AddControllerActionConfig(config);
            return config;
        }
    }
}