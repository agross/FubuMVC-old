using FubuMVC.Core.Results;

namespace FubuMVC.Core.Behaviors
{
    public interface IActionBehavior
    {
        IActionBehavior InsideBehavior { get; set; }

        IInvocationResult Result { get; set; }

        void Invoke<TInput>(TInput input)
            where TInput : class;
    }
}