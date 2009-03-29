using System;
using System.Collections.Generic;
using FubuMVC.Core.Behaviors;

namespace FubuMVC.Core.Controller
{
    public interface IControllerActionInvoker
    {
        void Invoke(Delegate actionDelegate, IDictionary<string, object> requestData);
    }

    public class ThunderdomeActionInvoker<CONTROLLER, INPUT, OUTPUT> : IControllerActionInvoker
        where CONTROLLER : class
        where INPUT : class, new()
        where OUTPUT : class
    {
        private readonly CONTROLLER _controller;

        public ThunderdomeActionInvoker(CONTROLLER controller, IControllerActionBehavior behavior)
        {
            _controller = controller;
            Behavior = behavior;
        }

        public IControllerActionBehavior Behavior { get; private set; }

        public virtual INPUT CreateActionInput(IDictionary<string, object> requestData)
        {
            ICollection<ConvertProblem> problems;
            var input = DictionaryConverter.CreateAndPopulate<INPUT>(requestData, out problems);

            //TODO: This should be handled better, has potential security implications, I think (not sure)
            if (problems.Count > 0)
            {
                throw new InvalidOperationException("Could not convert all dictionary values.");
            }

            return input;
        }

        public void Invoke(Delegate actionDelegate, IDictionary<string, object> requestData)
        {
            var input = CreateActionInput(requestData);
            var actionFunc = (Func<CONTROLLER, INPUT, OUTPUT>) actionDelegate;

            Behavior.Invoke(input, i => actionFunc(_controller, i));
        }
    }
}