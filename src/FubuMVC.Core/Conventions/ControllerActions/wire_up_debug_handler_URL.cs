using System.Linq;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Controller.Config;

namespace FubuMVC.Core.Conventions.ControllerActions
{
    public class wire_up_debug_handler_URL : IControllerActionConfigConvention
    {
        public const string DEBUG_URL = "__debug_controller_actions";

        public void Apply(ControllerActionConfig actionConfig)
        {
            if (actionConfig.PrimaryUrl != FubuConventions.PrimaryApplicationUrl) return;

            if (!actionConfig.GetBehaviors().Any(b => b == typeof(OutputDebugInformation))) return;

            actionConfig.AddOtherUrl(DEBUG_URL);
        }

        public FubuConventions FubuConventions { get; set; }
    }
}