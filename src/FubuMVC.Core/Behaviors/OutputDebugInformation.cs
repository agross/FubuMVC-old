using System;
using FubuMVC.Core.Controller;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Controller.Results;

namespace FubuMVC.Core.Behaviors
{
    public class OutputDebugInformation : IControllerActionBehavior
    {
        private readonly FubuConventions _conventions;
        private readonly FubuConfiguration _configuration;

        public OutputDebugInformation(FubuConventions conventions, FubuConfiguration configuration)
        {
            _conventions = conventions;
            _configuration = configuration;
        }

        public IControllerActionBehavior InsideBehavior { get; set; }

        public IInvocationResult Result{ get; set;}

        public TOutput Invoke<TInput, TOutput>(TInput input, Func<TInput, TOutput> func) where TInput : class where TOutput : class
        {
            var output = func(input);
            Result = 
                ResultOverride.IfAvailable(output) 
                ?? new RenderDebugInformationResult(_conventions, _configuration, RenderDebugInformationResult.CONTENT_TYPE);

            return output;
        }
    }
}