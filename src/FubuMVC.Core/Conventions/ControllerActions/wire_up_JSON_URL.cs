using FubuMVC.Core.Controller.Config;

namespace FubuMVC.Core.Conventions.ControllerActions
{
    public class wire_up_JSON_URL : IControllerActionConfigConvention
    {
        public void Apply(ControllerActionConfig actionConfig)
        {
            actionConfig.AddOtherUrl(actionConfig.PrimaryUrl + FubuConventions.DefaultJsonExtension);
        }

        public FubuConventions FubuConventions { get; set; }
    }
}