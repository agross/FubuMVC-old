using FubuMVC.Core.Controller;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Controller.Results;
using FubuMVC.Core.Routing;

namespace FubuMVC.Core.Behaviors
{
    public class output_as_json_if_requested : behavior_base_for_convenience
    {
        private readonly ICurrentRequest _request;
        private readonly FubuConventions _conventions;

        public output_as_json_if_requested(ICurrentRequest currentRequest, FubuConventions conventions)
        {
            _request = currentRequest;
            _conventions = conventions;
        }

        public override TOutput AfterInvocation<TOutput>(TOutput output, IInvocationResult insideResult)
        {
            if (_request.GetUrl().AbsolutePath.EndsWith(_conventions.DefaultJsonExtension))
            {
                Result = ResultOverride.IfAvailable(output) ?? new RenderJsonResult<TOutput>(output);
            }
            else
            {
                Result = insideResult;
            }
            return output;
        }
    }
}