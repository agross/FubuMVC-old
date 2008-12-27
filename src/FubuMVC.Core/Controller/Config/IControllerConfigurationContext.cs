namespace FubuMVC.Core.Controller.Config
{
    public interface IControllerConfigContext
    {
        ControllerActionConfig CurrentConfig { get; set; }
    }

    public class DefaultControllerConfigContext : IControllerConfigContext
    {
        public ControllerActionConfig CurrentConfig{ get; set; }
    }
}
