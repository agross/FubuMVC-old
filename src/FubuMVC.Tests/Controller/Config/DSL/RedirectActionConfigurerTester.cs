
using System;
using System.Reflection;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Controller.Config.DSL;
using FubuMVC.Core.Controller.Invokers;
using FubuMVC.Tests.Behaviors;
using NUnit.Framework;

namespace FubuMVC.Tests.Controller.Config.DSL
{
    [TestFixture]
    public class when_checking_if_RedirectActionConfigurer_can_configure_a_method
    {
        private RedirectActionConfigurer _configurer;

        [SetUp]
        public void SetUp()
        {
            _configurer = new RedirectActionConfigurer();
        }

        [Test]
        public void should_be_false_if_method_is_not_public()
        {
            var method = typeof(MethodDummy).GetMethod("PrivateMethod", BindingFlags.Instance | BindingFlags.NonPublic);
            _configurer.ShouldConfigure(method).ShouldBeFalse();
        }

        [Test]
        public void should_be_false_if_method_is_generic()
        {
            var method = typeof(MethodDummy).GetMethod("GenericMethod");
            _configurer.ShouldConfigure(method).ShouldBeFalse();
        }

        [Test]
        public void should_be_true_if_method_has_void_return()
        {
            var method = typeof(MethodDummy).GetMethod("VoidMethod");
            _configurer.ShouldConfigure(method).ShouldBeTrue();
        }

        [Test]
        public void should_be_false_if_method_has_no_params()
        {
            var method = typeof(MethodDummy).GetMethod("NoParamMethod");
            _configurer.ShouldConfigure(method).ShouldBeFalse();
        }

        [Test]
        public void should_be_false_if_method_has_a_param_that_violates_the_generic_criteria()
        {
            var method = typeof(MethodDummy).GetMethod("VoidInvalidMethod");
            _configurer.ShouldConfigure(method).ShouldBeFalse();
        }

        [Test]
        public void should_be_false_if_method_has_more_than_one_param()
        {
            var method = typeof(MethodDummy).GetMethod("ManyParamMethod");
            _configurer.ShouldConfigure(method).ShouldBeFalse();
        }

        [Test]
        public void should_be_false_if_method_has_value_type_param()
        {
            var method = typeof(MethodDummy).GetMethod("ValueParamMethod");
            _configurer.ShouldConfigure(method).ShouldBeFalse();
        }
    }

    [TestFixture]
    public class when_configuring_a_method_with_the_RedirectActionConfigurer
    {
        private RedirectActionConfigurer _configurer;
        private MethodInfo _method;
        private ControllerActionConfig _config;

        [SetUp]
        public void SetUp()
        {
            _configurer = new RedirectActionConfigurer();
            _method = typeof(MethodDummy).GetMethod("VoidMethod");
            _config = _configurer.Configure(_method);
        }

        [Test]
        public void should_pass_in_the_correct_action_method()
        {
            _config.ActionMethod.ShouldBeTheSameAs(_method);
        }

        [Test]
        public void should_pass_in_the_RedirectActionInvoker_as_the_invoker_type_with_the_correct_generic_params()
        {
            _config.InvokerType.ShouldEqual(typeof(RedirectActionInvoker<MethodDummy, TestViewModel>));
        }

        [Test]
        public void should_setup_the_actionfunc_delegate_as_Action_Controller_input()
        {
            _config.ActionDelegate.ShouldBeOfType<Action<MethodDummy, TestViewModel>>();
        }
    }
}