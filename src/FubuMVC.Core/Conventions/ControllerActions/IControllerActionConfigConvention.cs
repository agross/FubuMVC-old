using FubuMVC.Core.Controller.Config;

namespace FubuMVC.Core.Conventions.ControllerActions
{
    public interface IControllerActionConfigConvention : IFubuConvention<ControllerActionConfig>
    {
        FubuConventions FubuConventions { set; }
    }
}