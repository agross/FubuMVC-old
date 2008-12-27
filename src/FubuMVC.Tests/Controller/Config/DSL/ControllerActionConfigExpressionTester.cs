using System.Linq;
using FubuMVC.Core.Controller.Config.DSL;
using NUnit.Framework;
using FubuMVC.Core.Controller.Config;

namespace FubuMVC.Tests.Controller.Config.DSL
{
    [TestFixture]
    public class ControllerActionConfigExpressionTester
    {
        private ControllerActionConfigExpression<TestController> _expression;
        private FubuConventions _conventions;
        private FubuConfiguration _config;

        [SetUp]
        public void SetUp()
        {
            _conventions = new FubuConventions();
            _config = new FubuConfiguration(_conventions);
            _expression = new ControllerActionConfigExpression<TestController>(_config, _conventions);
        }

        [Test]
        public void Action_should_use_existing_config_if_present()
        {
            var oldConfig = ControllerActionConfig.ForAction<TestController, TestInputModel, TestOutputModel>((c, i) => c.SomeAction(i));
            _config.AddControllerActionConfig(oldConfig);

            _expression.Action<TestInputModel, TestOutputModel>((c, i) => c.SomeAction(i));

            _config.GetControllerActionConfigs().Single().ShouldBeTheSameAs(oldConfig);
        }

        [Test]
        public void Action_should_add_actionconfig_to_the_overall_config()
        {
            _expression.Action<TestInputModel, TestOutputModel>((c, i) => c.SomeAction(i));

            _config.GetControllerActionConfigs().ShouldHaveCount(1);
        }

        [Test]
        public void Action_should_set_up_the_default_primaryUrl_for_the_config()
        {
            _expression.Action<TestInputModel, TestOutputModel>((c, i) => c.SomeAction(i),
                                                                o => o.Config.PrimaryUrl.ShouldNotBeNull());
        }

        [Test]
        public void Action_should_use_the_UrlConventions_to_setup_the_primaryurl()
        {
            var testUrl = "testurl";

            _conventions.PrimaryUrlConvention = config => testUrl;

            _expression.Action<TestInputModel, TestOutputModel>((c, i) => c.SomeAction(i),
                                                                o => o.Config.PrimaryUrl.ShouldEqual(testUrl));
        }
    }
}