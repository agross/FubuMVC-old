using System.Reflection;

namespace FubuMVC.Core.Controller.Config.DSL
{
    public interface IControllerActionConfigurer
    {
        bool ShouldConfigure(MethodInfo method);
        ControllerActionConfig Configure(MethodInfo method);
    }
}