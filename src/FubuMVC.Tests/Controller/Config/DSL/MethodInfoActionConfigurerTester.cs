using System.Linq;
using System.Reflection;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Controller.Config.DSL;
using FubuMVC.Core.Util;
using NUnit.Framework;

namespace FubuMVC.Tests.Controller.Config.DSL
{
    [TestFixture]
    public class MethodInfoActionConfigurerTester
    {
        private MethodInfoActionConfigurer<ExpressionControllerTest, TestInputModel, TestOutputModel> _configurer;
        private MethodInfo _method;

        [SetUp]
        public void SetUp()
        {
            _configurer = new MethodInfoActionConfigurer<ExpressionControllerTest, TestInputModel, TestOutputModel>();
            _method = ReflectionHelper.GetMethod<ExpressionControllerTest>(c => c.MyAction(null));
        }

        [Test]
        public void ExpressionFrom_should_build_a_simple_lambda_expression_that_calls_the_underlying_action()
        {
            var expected = "foo";

            var expression = _configurer.ExpressionFrom(_method);

            var func = expression.Compile();

            var result = func(new ExpressionControllerTest(), new TestInputModel{Prop1=expected});

            result.Prop1.ShouldEqual(expected);
        }

        [Test]
        public void ForMethodInfo_should_return_a_controller_action_config_with_correct_types_and_action_reference()
        {
            var conventions = new FubuConventions();
            var fubuConfig = new FubuConfiguration(conventions);
            _configurer.Configure(_method, fubuConfig, conventions);

            var config = fubuConfig.GetControllerActionConfigs().Single();

            config.ControllerType.ShouldEqual(typeof (ExpressionControllerTest));
            config.InputType.ShouldEqual(typeof (TestInputModel));
            config.OutputType.ShouldEqual(typeof(TestOutputModel));
            config.ActionMethod.ShouldEqual(_method);
        }

        public class ExpressionControllerTest
        {
            public TestOutputModel MyAction(TestInputModel input)
            {
                return new TestOutputModel {Prop1 = input.Prop1};
            }
        }
    }
}