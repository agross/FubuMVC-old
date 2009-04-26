using FubuMVC.Core.Conventions;
using FubuMVC.Core.Conventions.ControllerActions;

namespace FubuMVC.Core.Controller.Config.DSL
{
    public class ActionConventionExpression
    {
        private readonly FubuConventions _fubuConventions;
        private readonly FubuConfiguration _fubuConfiguration;

        public ActionConventionExpression(FubuConventions fubuConventions, FubuConfiguration configuration)
        {
            _fubuConventions = fubuConventions;
            _fubuConfiguration = configuration;
        }

        public ActionConventionExpression Add<CONVENTION>()
            where CONVENTION : IFubuConvention<ControllerActionConfig>, new()
        {
            var conv = new CONVENTION();
            var cacConv = conv as IControllerActionConfigConvention;

            if (cacConv != null)
            {
                cacConv.FubuConventions = _fubuConventions;
            }

            _fubuConfiguration.AddConvention(conv);

            return this;
        }

        public ActionConventionExpression Add(IFubuConvention<ControllerActionConfig> convention)
        {
            _fubuConfiguration.AddConvention(convention);
            return this;
        }

    }
}