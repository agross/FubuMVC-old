using FubuMVC.Core.Controller;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Controller.Results;
using FubuMVC.Core.Conventions.ControllerActions;
using FubuMVC.Core.Routing;

namespace FubuMVC.Core.Behaviors
{
    public class OutputDebugInformation : behavior_base_for_convenience
    {
        private readonly ICurrentRequest _currentRequest;
        private readonly FubuConventions _conventions;
        private readonly FubuConfiguration _configuration;

        public OutputDebugInformation(ICurrentRequest currentRequest, FubuConventions conventions, FubuConfiguration configuration)
        {
            _currentRequest = currentRequest;
            _conventions = conventions;
            _configuration = configuration;
        }

        public override OUTPUT AfterInvocation<OUTPUT>(OUTPUT output, IInvocationResult insideResult)
        {
            var isDebug = _currentRequest.GetUrl().ToString().Contains(wire_up_debug_handler_URL.DEBUG_URL);

            if (isDebug)
            {
                Result = new RenderDebugInformationResult(_conventions, _configuration, RenderDebugInformationResult.CONTENT_TYPE);
                return output;
            }

            Result = insideResult;
            return output;
        }
    }
}