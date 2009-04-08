using System.Linq;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Controller.Config;

namespace FubuMVC.Core.Conventions.ControllerActions
{
    public class wire_up_JSON_URL_if_required : IControllerActionConfigConvention
    {
        public void Apply(ControllerActionConfig actionConfig)
        {
            if(! actionConfig.GetBehaviors().Any(b=>b == typeof(OutputAsJson))) return;

            actionConfig.AddOtherUrl(actionConfig.PrimaryUrl + FubuConventions.DefaultJsonExtension);
        }

        public FubuConventions FubuConventions { get; set; }
    }
}