using System;
using System.Collections.Generic;
using FubuMVC.Core.Controller.Invokers;
using NUnit.Framework;
using FubuMVC.Core.Behaviors;
using Rhino.Mocks;

namespace FubuMVC.Tests.Controller.Invokers
{
    [TestFixture]
    public class ThunderdomeActionInvokerTester
    {
        private ThunderdomeActionInvoker<TestController, TestInputModel, TestOutputModel> _invoker;
        private TestController _controller;
        private IControllerActionBehavior _behavior;

        [SetUp]
        public void SetUp()
        {
            _controller = new TestController();
            _behavior = MockRepository.GenerateMock<IControllerActionBehavior>();
            _invoker =
                new ThunderdomeActionInvoker<TestController, TestInputModel, TestOutputModel>(_controller, _behavior);
        }


        [Test]
        public void should_throw_exception_if_there_was_a_problem_populating_the_input_model()
        {
            typeof (InvalidOperationException).ShouldBeThrownBy(
                () => _invoker.Invoke(new Func<TestController, TestInputModel, TestOutputModel>(
                    (c, i) => null), new Dictionary<string, object>{{"PropInt", "BOGUS"}}));
        }

        [Test]
        public void invoke_executes_the_action_and_returns_a_result()
        {
            var testName = "TEST";
            var requestData = new Dictionary<string, object> { { "Prop1", testName } };

            _invoker.Invoke(new Func<TestController, TestInputModel, TestOutputModel>((c, i) => c.SomeAction(i)), requestData);

            _behavior.AssertWasCalled(b => b.Invoke<TestInputModel, TestOutputModel>(null, null), o => o.IgnoreArguments());
        }
    }
}