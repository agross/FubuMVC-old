using FubuMVC.Core.Controller;

namespace FubuMVC.Core.Behaviors
{
    public interface ISupportResultOverride
    {
        IInvocationResult ResultOverride { get; }
    }

    public static class ResultOverride
    {
        public static IInvocationResult IfAvailable<OUTPUT>(OUTPUT output)
        {
            var overrider = output as ISupportResultOverride;
            return overrider == null ? null : overrider.ResultOverride;
        }
    }

}