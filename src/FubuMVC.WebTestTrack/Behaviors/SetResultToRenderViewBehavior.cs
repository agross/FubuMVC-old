using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Results;

namespace FubuMVC.WebTestTrack.Behaviors
{
    public class SetResultToRenderViewBehavior : IActionBehavior
    {
        private readonly string _viewToRender;

        public SetResultToRenderViewBehavior(string viewToRender)
        {
            _viewToRender = viewToRender;
        }

        public IActionBehavior InsideBehavior { get; set; }
        public IInvocationResult Result { get; set; }
        public void Invoke<TInput>(TInput input) where TInput : class
        {
            Result = new RenderViewResult(_viewToRender);

            InsideBehavior.Invoke(input);
        }
    }
}