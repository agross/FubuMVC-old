using System;
using System.Collections.Generic;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Controller.Results;

namespace FubuMVC.Core.Controller.Invokers
{
    public class RedirectActionInvoker<TController, TInput> : IControllerActionInvoker 
        where TInput : class, new()
    {
        private readonly TController _controller;
        private readonly IControllerActionBehavior _behavior;
        private string _redirectsToUrl;

        public RedirectActionInvoker(TController controller, IControllerActionBehavior behavior, string redirectsToUrl)
        {
            _controller = controller;
            _behavior = behavior;
            _redirectsToUrl = redirectsToUrl;
        }

        public void Invoke(Delegate actionDelegate, IDictionary<string, object> requestData)
        {
            var input = DictionaryConverter.SafeCreateAndPopulate<TInput>(requestData);

            var actionFunc = (Action<TController, TInput>)actionDelegate;

            _behavior.Result = new RedirectResult(_redirectsToUrl);

            _behavior.Invoke<TInput, object>(input, i => 
            { 
                actionFunc(_controller, i);
                return null; 
            });
        }
    }
}