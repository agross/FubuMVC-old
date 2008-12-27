using FubuMVC.Core.Controller;
using Microsoft.Practices.ServiceLocation;

namespace FubuMVC.Core.Behaviors
{
    public class execute_the_result : behavior_base_for_convenience
    {
        private readonly IServiceLocator _locator;

        public execute_the_result(IServiceLocator locator)
        {
            _locator = locator;
        }

        public override OUTPUT AfterInvocation<OUTPUT>(OUTPUT output, IInvocationResult insideResult)
        {
            insideResult.Execute(_locator);
            return output;
        }
    }
}