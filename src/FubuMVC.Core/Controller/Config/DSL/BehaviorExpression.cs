using FubuMVC.Core.Behaviors;

namespace FubuMVC.Core.Controller.Config.DSL
{
    public class BehaviorExpression
    {
        private readonly FubuConfiguration _config;

        public BehaviorExpression(FubuConfiguration configuration)
        {
            _config = configuration;
        }

        public BehaviorExpression Will<BEHAVIOR>()
            where BEHAVIOR : IControllerActionBehavior
        {
            _config.AddDefaultBehavior<BEHAVIOR>();
            
            return this;
        }
    }
}