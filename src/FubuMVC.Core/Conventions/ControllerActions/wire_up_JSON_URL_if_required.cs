using System.Linq;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Controller.Config;

namespace FubuMVC.Core.Conventions.ControllerActions
{
    public class wire_up_JSON_URL_if_required : IFubuConvention<ControllerActionConfig>
    {
        private readonly FubuConventions _conventions;

        public wire_up_JSON_URL_if_required(FubuConventions conventions)
        {
            _conventions = conventions;
        }

        public void Apply(ControllerActionConfig actionConfig)
        {
            if(! actionConfig.GetBehaviors().Any(b=>b == typeof(OutputAsJson))) return;

            actionConfig.AddOtherUrl(actionConfig.PrimaryUrl + _conventions.DefaultJsonExtension);
        }
    }
}