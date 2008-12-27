using System;
using FubuMVC.Core.Controller;

namespace FubuMVC.Core.Behaviors
{
    public interface IControllerActionBehavior
    {
        IControllerActionBehavior InsideBehavior { get; set; }

        IInvocationResult Result { get; set; }

        OUTPUT Invoke<INPUT, OUTPUT>(INPUT input, Func<INPUT, OUTPUT> func)
            where INPUT : class
            where OUTPUT : class;
    }
}