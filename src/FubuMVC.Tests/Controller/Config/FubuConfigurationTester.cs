using System;
using System.Linq;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Conventions;
using FubuMVC.Core.Util;
using FubuMVC.Tests.Controller.Config.DSL;
using NUnit.Framework;

namespace FubuMVC.Tests.Controller.Config
{
    [TestFixture]
    public class FubuConfigurationTester
    {
        private FubuConfiguration _fubuConfig;
        private ControllerActionConfig _config;

        [SetUp]
        public void SetUp()
        {
            _fubuConfig = new FubuConfiguration(new FubuConventions());
            var method = ReflectionHelper.GetMethod<TestController>(c => c.SomeAction(null));
            _config = new ControllerActionConfig(method, null, null);

            _fubuConfig.AddControllerActionConfig(_config);
        }

        [Test]
        public void AddDefaultBehavior_should_add_the_behavior_to_the_list()
        {
            _fubuConfig.AddDefaultBehavior<TestBehavior>();

            _fubuConfig.GetDefaultBehaviors().ShouldHaveTheSameElementsAs(typeof(TestBehavior));
        }

        [Test]
        public void AddDefaultBehavior_should_not_add_duplicates()
        {
            _fubuConfig.AddDefaultBehavior<TestBehavior>();
            _fubuConfig.AddDefaultBehavior<TestBehavior>();

            _fubuConfig.GetDefaultBehaviors().ShouldHaveTheSameElementsAs(typeof(TestBehavior));
        }

        [Test]
        public void AddConvention_should_add_the_behavior_to_the_list()
        {
            var convention = new UrlHappyConventionForTestPurposes();
            _fubuConfig.AddConvention(convention);

            _fubuConfig.GetActionConventions().ShouldHaveTheSameElementsAs(convention);
        }


        [Test]
        public void GetDefaultBehaviors_should_return_elements_in_same_order_as_added()
        {
            _fubuConfig.AddDefaultBehavior<TestBehavior2>();
            _fubuConfig.AddDefaultBehavior<TestBehavior>();

            _fubuConfig.GetDefaultBehaviors().ShouldHaveTheSameElementsAs(typeof(TestBehavior2), typeof(TestBehavior));
        }

        [Test]
        public void AddControllerActionConfig_should_add_the_config_to_the_list()
        {
            _fubuConfig.GetControllerActionConfigs().ShouldHaveTheSameElementsAs(_config);
        }

        [Test]
        public void AddControllerActionConfig_should_apply_default_behaviors_to_the_configuration()
        {
            _fubuConfig = new FubuConfiguration(new FubuConventions());

            _fubuConfig.AddDefaultBehavior<TestBehavior2>();
            _fubuConfig.AddDefaultBehavior<TestBehavior>();

            _fubuConfig.AddControllerActionConfig(_config);

            _config.GetBehaviors().ShouldHaveTheSameElementsAs(typeof(TestBehavior2), typeof(TestBehavior));
        }

        [Test]
        public void AddControllerActionConfig_should_apply_conventions_after_default_behaviors()
        {
            var convention = new TestBehaviorCheckingConvention();

            _fubuConfig = new FubuConfiguration(new FubuConventions());
            _fubuConfig.AddDefaultBehavior<TestBehavior>();
            _fubuConfig.AddConvention(convention);
            _fubuConfig.AddControllerActionConfig(_config);

            convention.TestBehaviorFound.ShouldBeTrue();
        }

        private class TestBehaviorCheckingConvention :IFubuConvention<ControllerActionConfig>
        {
            public void Apply(ControllerActionConfig item)
            {
                TestBehaviorFound = item.GetBehaviors().Any(t => t == typeof (TestBehavior));
            }

            public bool TestBehaviorFound { get; set; }
        }

        [Test]
        public void AddControllerActionConfig_should_throw_if_config_has_same_controller_type_and_action_as_another_config()
        {
            var method = ReflectionHelper.GetMethod<TestController>(c => c.SomeAction(null));
            var dupeConfig = new ControllerActionConfig(method, null, null);

            typeof (InvalidOperationException).ShouldBeThrownBy(
                () => _fubuConfig.AddControllerActionConfig(dupeConfig));
        }

        [Test]
        public void GetConfigForAction_should_find_the_action_config()
        {
            var foundConfig = _fubuConfig.GetConfigForAction<TestController>(c => c.SomeAction(null));

            foundConfig.ShouldBeTheSameAs(_config);
        }

        [Test]
        public void GetConfigForAction_should_not_find_non_existant_config()
        {
            var foundConfig = _fubuConfig.GetConfigForAction<TestController>(c => c.AnotherAction(null));

            foundConfig.ShouldBeNull();
        }

        [Test]
        public void GetDefaultUrlFor_controller_should_return_the_conventional_URL_for_this_controller()
        {
            _fubuConfig.GetDefaultUrlFor<TestController>().ShouldEqual("test");
        }

        [Test]
        public void GetUrlFormatFor_should_return_the_base_url_format()
        {
            _config.PrimaryUrl = "boo";
            _fubuConfig.GetDefaultUrlFor(_config).ShouldEqual("boo");
        }

        [Test]
        public void GetUrlFormatFor_should_return_the_base_url_format_for_an_action_expression()
        {
            _config.PrimaryUrl = "boo";
            _fubuConfig.GetDefaultUrlFor<TestController>(c => c.SomeAction(null)).ShouldEqual("boo");
        }

    }
}