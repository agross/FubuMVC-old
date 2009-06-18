using Microsoft.Practices.ServiceLocation;
using FubuMVC.Core.Routing;

namespace FubuMVC.Core.Results
{
    public class RedirectResult : IInvocationResult
    {
        public RedirectResult(string url)
        {
            Url = url;
        }

        public string Url { get; set; }

        public void Execute(IServiceLocator locator)
        {
            var writer = locator.GetInstance<IOutputWriter>();
            writer.RedirectToUrl(Url);
        }
    }
}