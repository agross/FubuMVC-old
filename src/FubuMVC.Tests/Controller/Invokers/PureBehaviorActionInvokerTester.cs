using System.Collections.Generic;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Controller.Invokers;
using NUnit.Framework;
using Rhino.Mocks;

namespace FubuMVC.Tests.Controller.Invokers
{
    [TestFixture]
    public class PureBehaviorActionInvokerTester
    {
        private IControllerActionBehavior _behavior;
        private PureBehaviorActionInvoker<object, object> _invoker;

        [Test]
        public void should_invoke_the_first_behavior()
        {
            _behavior = MockRepository.GenerateStub<IControllerActionBehavior>();
            _invoker = new PureBehaviorActionInvoker<object, object>(_behavior);
            _invoker.Invoke(new Dictionary<string, object>());

            _behavior.AssertWasCalled(b => b.Invoke<object, object>(null, null), o => o.IgnoreArguments());
        }
    }
}