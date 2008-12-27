using System;
using FubuMVC.Core.Controller;
using FubuMVC.Core.Controller.Results;

namespace FubuMVC.Core.Behaviors
{
    public class DefaultBehavior : IControllerActionBehavior
    {
        public IControllerActionBehavior InsideBehavior{ get; set; }
        public IInvocationResult Result { get; set; }

        public OUTPUT Invoke<INPUT, OUTPUT>(INPUT input, Func<INPUT, OUTPUT> func) 
            where INPUT : class 
            where OUTPUT : class
        {
            var output = func(input);

            Result = ResultOverride.IfAvailable(output) ?? new RenderViewResult<OUTPUT>(output);

            return output;
        }
    }
}