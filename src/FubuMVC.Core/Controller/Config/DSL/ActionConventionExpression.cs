using System.Collections.Generic;
using FubuMVC.Core.Conventions;

namespace FubuMVC.Core.Controller.Config.DSL
{
    public class ActionConventionExpression
    {
        private readonly IList<IFubuConvention<ControllerActionConfig>> _conventions = new List<IFubuConvention<ControllerActionConfig>>();

        public IEnumerable<IFubuConvention<ControllerActionConfig>> Conventions { get { return _conventions; } }

        public ActionConventionExpression Add<CONVENTION>()
            where CONVENTION : IFubuConvention<ControllerActionConfig>, new()
        {
            _conventions.Add(new CONVENTION());
            return this;
        }

        public ActionConventionExpression Add(IFubuConvention<ControllerActionConfig> convention)
        {
            _conventions.Add(convention);
            return this;
        }

    }
}