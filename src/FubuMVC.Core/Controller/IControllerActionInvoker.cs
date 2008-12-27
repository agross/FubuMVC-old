using System;
using System.Collections.Generic;
using FubuMVC.Core.Behaviors;
using Microsoft.Practices.ServiceLocation;

namespace FubuMVC.Core.Controller
{
    public interface IControllerActionInvoker<CONTROLLER, INPUT, OUTPUT>
        where CONTROLLER : class
        where INPUT : class, new()
        where OUTPUT : class
    {
        void Invoke(Func<CONTROLLER, INPUT, OUTPUT> actionFunc, IDictionary<string, object> requestData);
    }

    public class ThunderdomeActionInvoker<CONTROLLER, INPUT, OUTPUT> : IControllerActionInvoker<CONTROLLER, INPUT, OUTPUT>
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

        public void Invoke(Func<CONTROLLER, INPUT, OUTPUT> actionFunc, IDictionary<string, object> requestData)
        {
            var input = CreateActionInput(requestData);

            Behavior.Invoke(input, i => actionFunc(_controller, i));
        }
    }
}