using System.Collections.Generic;
using FubuMVC.Core.Behaviors;

namespace FubuMVC.Core.Runtime.Invokers
{
    public class PureBehaviorActionInvoker<TInput> : IActionInvoker
        where TInput : class, new()
    {
        private readonly IDictionaryConverter<TInput> _converter;
        private readonly IActionBehavior _behavior;

        public PureBehaviorActionInvoker(IDictionaryConverter<TInput> converter, IActionBehavior behavior)
        {
            _converter = converter;
            _behavior = behavior;
        }

        public void Invoke(IDictionary<string, object> dictionary)
        {
            ICollection<ConvertProblem> problems;
            var input = _converter.ConvertFrom(dictionary, out problems);

            //TODO: need to deal with the ConvertProblems

            _behavior.Invoke(input);
        }
    }
}