using System.Collections.Generic;
using FubuMVC.Core.Conventions;
using FubuMVC.Core.Conventions.ControllerActions;

namespace FubuMVC.Core.Controller.Config.DSL
{
    public class ActionConventionExpression
    {
        private readonly IList<IFubuConvention<ControllerActionConfig>> _conventions = new List<IFubuConvention<ControllerActionConfig>>();
        private readonly FubuConventions _fubuConventions;

        public ActionConventionExpression(FubuConventions fubuConventions)
        {
            _fubuConventions = fubuConventions;
        }

        public IEnumerable<IFubuConvention<ControllerActionConfig>> Conventions { get { return _conventions; } }

        public ActionConventionExpression Add<CONVENTION>()
            where CONVENTION : IFubuConvention<ControllerActionConfig>, new()
        {
            var conv = new CONVENTION();
            var cacConv = conv as IControllerActionConfigConvention;

            if (cacConv != null)
            {
                cacConv.FubuConventions = _fubuConventions;
            }

            _conventions.Add(conv);
            return this;
        }

        public ActionConventionExpression Add(IFubuConvention<ControllerActionConfig> convention)
        {
            _conventions.Add(convention);
            return this;
        }

    }
}