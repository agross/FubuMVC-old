using System;
using System.Collections.Generic;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Controller.Results;

namespace FubuMVC.Core.Controller.Invokers
{
    public class RedirectActionInvoker<TController, TInput> : IControllerActionInvoker 
        where TInput : class, new()
    {
        private readonly TController _controller;
        private readonly IControllerActionBehavior _behavior;
        private readonly FubuConventions _conventions;

        public RedirectActionInvoker(TController controller, IControllerActionBehavior behavior, FubuConventions conventions)
        {
            _controller = controller;
            _behavior = behavior;
            _conventions = conventions;
        }

        public void Invoke(Delegate actionDelegate, IDictionary<string, object> requestData)
        {
            var input = DictionaryConverter.SafeCreateAndPopulate<TInput>(requestData);

            var actionFunc = (Action<TController, TInput>)actionDelegate;

            _behavior.Result = new RedirectResult(_conventions.PrimaryApplicationUrl);

            _behavior.Invoke<TInput, object>(input, i => 
            { 
                actionFunc(_controller, i);
                return null; 
            });
        }
    }
}