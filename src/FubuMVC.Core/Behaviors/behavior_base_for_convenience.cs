using System;
using FubuMVC.Core.Controller;

namespace FubuMVC.Core.Behaviors
{
    public abstract class behavior_base_for_convenience : IControllerActionBehavior
    {
        public IControllerActionBehavior InsideBehavior{ get; set; }
        public IInvocationResult Result { get; set; }

        public virtual void PrepareInput<INPUT>(INPUT input)
            where INPUT : class
        {
        }

        public virtual OUTPUT PerformInvoke<INPUT, OUTPUT>(INPUT input, Func<INPUT, OUTPUT> func)
            where INPUT : class 
            where OUTPUT : class
        {
            return InsideBehavior.Invoke(input, func);
        }

        public virtual void ModifyOutput<OUTPUT>(OUTPUT output)
            where OUTPUT : class
        {
        }

        public virtual OUTPUT AfterInvocation<OUTPUT>(OUTPUT output, IInvocationResult insideResult)
            where OUTPUT : class
        {
            Result = insideResult;
            return output;
        }

        public OUTPUT Invoke<INPUT, OUTPUT>(INPUT input, Func<INPUT, OUTPUT> func) 
            where INPUT : class 
            where OUTPUT : class
        {
            PrepareInput(input);
            var output = PerformInvoke(input, func);
            ModifyOutput(output);
            return AfterInvocation(output, InsideBehavior.Result);
        }
    }
}