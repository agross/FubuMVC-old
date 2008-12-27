using System;
using System.Collections.Generic;
using NUnit.Framework;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Controller;

namespace FubuMVC.Tests.Controller
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
            _behavior = new DefaultBehavior();
            _invoker =
                new ThunderdomeActionInvoker<TestController, TestInputModel, TestOutputModel>(_controller, _behavior);
        }


        [Test]
        public void should_throw_exception_if_there_was_a_problem_populating_the_input_model()
        {
            typeof (InvalidOperationException).ShouldBeThrownBy(() => _invoker.Invoke((c, i) => null, new Dictionary<string, object>{{"PropInt", "BOGUS"}}));
        }

        [Test]
        public void invoke_executes_the_action_and_returns_a_result()
        {
            var testName = "TEST";
            var requestData = new Dictionary<string, object> { { "Prop1", testName } };

            var output = _invoker.Invoke((c, i) => c.SomeAction(i), requestData);
            output.ShouldBeOfType<IInvocationResult>();
        }
    }
}