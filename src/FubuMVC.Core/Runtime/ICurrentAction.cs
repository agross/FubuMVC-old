using FubuMVC.Core.Config;

namespace FubuMVC.Core.Runtime
{
    public interface ICurrentAction
    {
        UrlAction Current{ get; set;  }
    }

    public class CurrentAction : ICurrentAction
    {
        public UrlAction Current { get; set; }
    }
}