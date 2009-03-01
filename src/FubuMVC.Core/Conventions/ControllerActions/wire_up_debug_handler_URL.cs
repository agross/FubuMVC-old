using System;
using FubuMVC.Core.Controller.Config;

namespace FubuMVC.Core.Conventions.ControllerActions
{
    public class wire_up_debug_handler_URL : IFubuConvention<ControllerActionConfig>
    {
        public const string DEBUG_URL = "__debug_controller_actions";

        public void Apply(ControllerActionConfig actionConfig)
        {
            if( ! actionConfig.ControllerType.Name.StartsWith("Debug") 
                || ! actionConfig.ActionName.Equals("Index", StringComparison.OrdinalIgnoreCase)) return;

            actionConfig.PrimaryUrl = DEBUG_URL;
        }
    }
}