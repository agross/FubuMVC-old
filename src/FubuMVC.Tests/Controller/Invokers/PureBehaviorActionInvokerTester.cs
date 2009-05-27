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
        private PureBehaviorActionInvoker<TestInputModel, TestOutputModel> _invoker;

        [SetUp]
        public void SetUp()
        {
            _behavior = MockRepository.GenerateStub<IControllerActionBehavior>();
            _invoker = new PureBehaviorActionInvoker<TestInputModel, TestOutputModel>(_behavior);
        }

        [Test]
        public void should_invoke_the_first_behavior()
        {
            _invoker.Invoke(new Dictionary<string, object>());

            _behavior.AssertWasCalled(b => b.Invoke<TestInputModel, TestOutputModel>(null, null), o => o.IgnoreArguments());
        }

        [Test]
        public void should_populate_the_input_model()
        {
            var dict = new Dictionary<string, object> {{"PropInt", 4}};

            var catcher = _behavior.CaptureArgumentsFor(b => b.Invoke<TestInputModel, TestOutputModel>(null, null), o=>o.Return(new TestOutputModel()));
            
            _invoker.Invoke(dict);

            catcher.First<TestInputModel>().PropInt.ShouldEqual(4);
        }
    }
}