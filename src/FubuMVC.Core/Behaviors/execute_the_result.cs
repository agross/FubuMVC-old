using FubuMVC.Core.Controller;
using FubuMVC.Core.Controller.Results;
using Microsoft.Practices.ServiceLocation;

namespace FubuMVC.Core.Behaviors
{
    public class execute_the_result : behavior_base_for_convenience
    {
        private readonly IServiceLocator _locator;
        private readonly IResultOverride _overrider;

        public execute_the_result(IServiceLocator locator, IResultOverride overrider)
        {
            _locator = locator;
            _overrider = overrider;
        }

        public override OUTPUT AfterInvocation<OUTPUT>(OUTPUT output, IInvocationResult insideResult)
        {
            var result = _overrider.OverrideResult ?? insideResult;

            result.Execute(_locator);
            return output;
        }
    }
}