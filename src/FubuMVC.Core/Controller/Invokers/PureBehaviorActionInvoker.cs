using System.Collections.Generic;
using FubuMVC.Core.Behaviors;

namespace FubuMVC.Core.Controller.Invokers
{
    public class PureBehaviorActionInvoker<TInput, TOutput> : IControllerActionInvoker
        where TInput : class, new()
        where TOutput : class
    {
        private readonly IControllerActionBehavior _behavior;

        public PureBehaviorActionInvoker(IControllerActionBehavior behavior)
        {
            _behavior = behavior;
        }

        public void Invoke(IDictionary<string, object> requestData)
        {
            var input = DictionaryConverter.SafeCreateAndPopulate<TInput>(requestData);

            _behavior.Invoke(input, i => default(TOutput));
        }
    }
}