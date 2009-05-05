using System;
using System.Collections.Generic;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Controller.Invokers;
using FubuMVC.Core.Controller.Results;
using NUnit.Framework;
using Rhino.Mocks;

namespace FubuMVC.Tests.Controller.Invokers
{
    [TestFixture]
    public class when_supplying_invalid_input_data_to : the_RedirectActionInvoker_should
    {
        [Test]
        public void throw_exception_when_there_are_converter_errors()
        {
            typeof(InvalidOperationException).ShouldBeThrownBy(
                () => _invoker.Invoke(new Action<TestController, TestInputModel>(
                    (c, i) => c.RedirectAction(null)), new Dictionary<string, object>
                        {
                            { "PropInt", "BOGUS" }
                        }));
        }
    }

    [TestFixture]
    public class when_invoking_with_a_valid_redirect_url : the_RedirectActionInvoker_should
    {
        private RedirectResult _result;

        protected override void BeforeEach()
        {
            _invoker.Invoke(new Action<TestController, TestInputModel>(
                (c, i) => c.RedirectAction(i)),
                new Dictionary<string, object>());
            _result = _behavior.Result as RedirectResult;
        } 

        [Test]
        public void invoke_the_action_and_set_up_a_redirect_result()
        {
            _result.ShouldNotBeNull();
        }

        [Test]
        public void redirect_to_the_default_application_url_by_default()
        {
            _result.Url.ShouldEqual(_expectedUrl);
        }
    }

    public class the_RedirectActionInvoker_should
    {
        protected RedirectActionInvoker<TestController, TestInputModel> _invoker;
        protected IControllerActionBehavior _behavior;
        protected TestController _controller;
        protected string _expectedUrl;
        
        [SetUp]
        public void SetUp()
        {
            _controller = new TestController();
            _behavior = MockRepository.GenerateStub<IControllerActionBehavior>();
            _expectedUrl = "EXPECTED_URL";
            var conventions = new FubuConventions {PrimaryApplicationUrl = _expectedUrl};
            _invoker = new RedirectActionInvoker<TestController, TestInputModel>(_controller, _behavior, conventions);
            BeforeEach();
        }

        protected virtual void BeforeEach(){ }

    }
}