using System;
using FubuMVC.Core.Controller.Config;

namespace FubuMVC.Core.Conventions.ControllerActions
{
    public class wire_up_404_handler_URL : IControllerActionConfigConvention
    {
        public void Apply(ControllerActionConfig actionConfig)
        {
            if( ! actionConfig.ControllerType.Name.StartsWith("PageNotFound") 
                || ! actionConfig.ActionName.Equals("Index", StringComparison.OrdinalIgnoreCase)) return;

            actionConfig.PrimaryUrl = FubuConventions.PageNotFoundUrl;
        }

        public FubuConventions FubuConventions{ get; set;}
    }
}