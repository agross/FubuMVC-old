using System;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Results;

namespace FubuMVC.WebTestTrack.Behaviors
{
    public class ZMIZMOBehavior<TController> : IActionBehavior
    {
        private readonly TController _controller;
        private readonly Action<TController> _controllerAction;

        public ZMIZMOBehavior(TController controller, Action<TController> controllerAction)
        {
            _controller = controller;
            _controllerAction = controllerAction;
        }

        public IActionBehavior InsideBehavior { get; set; }
        public IInvocationResult Result { get; set; }

        public void Invoke<TInput>(TInput input) where TInput : class
        {
            _controllerAction(_controller);
        }
    }
}