using System;
using System.Collections.Generic;
using FubuMVC.Core.Behaviors;

namespace FubuMVC.Core.Controller.Invokers
{
    public class ThunderdomeActionInvoker<TController, TInput, TOutput> : IControllerActionInvoker
        where TController : class
        where TInput : class, new()
        where TOutput : class
    {
        private readonly TController _controller;

        public ThunderdomeActionInvoker(TController controller, IControllerActionBehavior behavior)
        {
            _controller = controller;
            Behavior = behavior;
        }

        public IControllerActionBehavior Behavior { get; private set; }

        public void Invoke(Delegate actionDelegate, IDictionary<string, object> requestData)
        {
            var input = DictionaryConverter.SafeCreateAndPopulate<TInput>(requestData);
            var actionFunc = (Func<TController, TInput, TOutput>) actionDelegate;

            Behavior.Invoke(input, i => actionFunc(_controller, i));
        }
    }
}