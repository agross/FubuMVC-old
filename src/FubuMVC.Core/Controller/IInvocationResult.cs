using Microsoft.Practices.ServiceLocation;

namespace FubuMVC.Core.Controller
{
    public interface IInvocationResult
    {
        void Execute(IServiceLocator locator);
    }
}
