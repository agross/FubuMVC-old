using System;
using System.Reflection;
using FubuMVC.Core.Controller;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Controller.Config.DSL;
using FubuMVC.Tests.Behaviors;
using NUnit.Framework;

namespace FubuMVC.Tests.Controller.Config.DSL
{
    [TestFixture]
    public class when_checking_if_ThunderdomeActionConfigurer_can_configure_a_method
    {
        private ThunderdomeActionConfigurer _configurer;

        [SetUp]
        public void SetUp()
        {
            _configurer = new ThunderdomeActionConfigurer();
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
        public void should_be_false_if_method_has_void_return()
        {
            var method = typeof(MethodDummy).GetMethod("VoidMethod");
            _configurer.ShouldConfigure(method).ShouldBeFalse();
        }

        [Test]
        public void should_be_false_if_method_has_no_params()
        {
            var method = typeof(MethodDummy).GetMethod("NoParamMethod");
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

        [Test]
        public void should_be_false_if_method_has_value_type_return_type()
        {
            var method = typeof(MethodDummy).GetMethod("ValueReturnMethod");
            _configurer.ShouldConfigure(method).ShouldBeFalse();
        }

        [Test]
        public void should_be_true_if_the_method_is_true_OMIOMO_method()
        {
            var method = typeof(MethodDummy).GetMethod("GoodMethod");
            _configurer.ShouldConfigure(method).ShouldBeTrue();
        }
    }

    public class MethodDummy
    {
        private string PrivateMethod(string blah) { return ""; }
        public string GenericMethod<T>(string blah) { return ""; }
        public void VoidMethod(string blah) { }
        public string NoParamMethod() { return ""; }
        public string ManyParamMethod(string blah, string foo) { return ""; }
        public string ValueParamMethod(int blah) { return ""; }
        public int ValueReturnMethod(string blah) { return 0; }
        public string GoodMethod(TestViewModel blah) { return ""; }
    }


    [TestFixture]
    public class when_configuring_a_method
    {
        private ThunderdomeActionConfigurer _configurer;
        private MethodInfo _method;
        private ControllerActionConfig _config;

        [SetUp]
        public void SetUp()
        {
            _configurer = new ThunderdomeActionConfigurer();
            _method = typeof(MethodDummy).GetMethod("GoodMethod");
            _config = _configurer.Configure(_method);
        }

        [Test]
        public void should_pass_in_the_correct_action_method()
        {
            _config.ActionMethod.ShouldBeTheSameAs(_method);
        }

        [Test]
        public void should_pass_in_the_ThunderdomeActionInvoker_as_the_invoker_type_with_the_correct_generic_params()
        {
            _config.InvokerType.ShouldEqual(typeof(ThunderdomeActionInvoker<MethodDummy, TestViewModel, string>));
        }

        [Test]
        public void should_setup_the_actionfunc_delegate_as_Func_Controller_input_output()
        {
            _config.ActionDelegate.ShouldBeOfType<Func<MethodDummy, TestViewModel, string>>();
        }
    }
}