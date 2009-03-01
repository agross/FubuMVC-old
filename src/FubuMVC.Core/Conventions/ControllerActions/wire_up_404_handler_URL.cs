using System;
using FubuMVC.Core.Controller.Config;

namespace FubuMVC.Core.Conventions.ControllerActions
{
    public class wire_up_404_handler_URL : IFubuConvention<ControllerActionConfig>
    {
        private readonly FubuConventions _conventions;

        public wire_up_404_handler_URL(FubuConventions conventions)
        {
            _conventions = conventions;
        }

        public void Apply(ControllerActionConfig actionConfig)
        {
            if( ! actionConfig.ControllerType.Name.StartsWith("PageNotFound") 
                || ! actionConfig.ActionName.Equals("Index", StringComparison.OrdinalIgnoreCase)) return;

            actionConfig.PrimaryUrl = _conventions.PageNotFoundUrl;
        }
    }
}