using Microsoft.Practices.ServiceLocation;

namespace FubuMVC.Core.Results
{
    public interface IInvocationResult
    {
        void Execute(IServiceLocator locator);
    }
}