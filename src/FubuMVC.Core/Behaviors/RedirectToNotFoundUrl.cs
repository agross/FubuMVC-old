using System;
using FubuMVC.Core.Controller;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Controller.Results;

namespace FubuMVC.Core.Behaviors
{
    public class RedirectToNotFoundUrl : IControllerActionBehavior
    {
        private readonly FubuConventions _conventions;

        public RedirectToNotFoundUrl(FubuConventions conventions)
        {
            _conventions = conventions;
        }

        public IControllerActionBehavior InsideBehavior { get; set; }

        public IInvocationResult Result{ get; set;}

        public TOutput Invoke<TInput, TOutput>(TInput input, Func<TInput, TOutput> func) where TInput : class where TOutput : class
        {
            var output = func(input);

            Result = new RedirectResult(_conventions.PageNotFoundUrl);

            return output;
        }
    }
}