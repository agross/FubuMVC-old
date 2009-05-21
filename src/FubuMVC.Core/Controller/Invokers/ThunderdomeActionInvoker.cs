using System;
using System.Collections.Generic;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Controller.Config;

namespace FubuMVC.Core.Controller.Invokers
{
    public class ThunderdomeActionInvoker<TController, TInput, TOutput> : IControllerActionInvoker
        where TController : class
        where TInput : class, new()
        where TOutput : class
    {
        private readonly TController _controller;
        private readonly IControllerConfigContext _context;

        public ThunderdomeActionInvoker(TController controller, IControllerActionBehavior behavior, IControllerConfigContext context)
        {
            _controller = controller;
            Behavior = behavior;
            _context = context;
        }

        public IControllerActionBehavior Behavior { get; private set; }

        public void Invoke(IDictionary<string, object> requestData)
        {
            var input = DictionaryConverter.SafeCreateAndPopulate<TInput>(requestData);
            var actionDelegate = _context.CurrentConfig.ActionDelegate;
            var actionFunc = (Func<TController, TInput, TOutput>) actionDelegate;

            Behavior.Invoke(input, i => actionFunc(_controller, i));
        }
    }
}