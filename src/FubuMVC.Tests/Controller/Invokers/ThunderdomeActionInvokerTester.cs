using System;
using System.Collections.Generic;
using FubuMVC.Core.Controller.Config;
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
        private IControllerConfigContext _context;
        private ControllerActionConfig _curConfig;

        [SetUp]
        public void SetUp()
        {
            _controller = new TestController();
            _behavior = MockRepository.GenerateMock<IControllerActionBehavior>();
            _context = MockRepository.GenerateStub<IControllerConfigContext>();
            _curConfig = _context.CurrentConfig =
                new ControllerActionConfig(
                    typeof (ThunderdomeActionInvoker<TestController, TestInputModel, TestOutputModel>));
            _invoker =

                new ThunderdomeActionInvoker<TestController, TestInputModel, TestOutputModel>(_controller, _behavior, _context);
        }


        [Test]
        public void should_throw_exception_if_there_was_a_problem_populating_the_input_model()
        {
            _curConfig.ActionDelegate = new Func<TestController, TestInputModel, TestOutputModel>(
                (c, i) => null);

            typeof (InvalidOperationException).ShouldBeThrownBy(
                () => _invoker.Invoke(new Dictionary<string, object>{{"PropInt", "BOGUS"}}));
        }

        [Test]
        public void invoke_executes_the_action_and_returns_a_result()
        {
            var testName = "TEST";
            var requestData = new Dictionary<string, object> { { "Prop1", testName } };
            _curConfig.ActionDelegate =
                new Func<TestController, TestInputModel, TestOutputModel>((c, i) => c.SomeAction(i));

            _invoker.Invoke(requestData);

            _behavior.AssertWasCalled(b => b.Invoke<TestInputModel, TestOutputModel>(null, null), o => o.IgnoreArguments());
        }
    }
}