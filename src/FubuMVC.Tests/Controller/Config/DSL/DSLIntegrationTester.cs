using System;
using System.Linq;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Controller.Config.DSL;
using NUnit.Framework;

namespace FubuMVC.Tests.Controller.Config.DSL
{
    [TestFixture]
    public class DSLIntegrationTester
    {
        private FubuConfiguration _config;
        private FubuConventions _conventions;
        private ControllerActionDSL _dsl;

        [SetUp]
        public void SetUp()
        {
            _conventions = new FubuConventions();
            _config = new FubuConfiguration(_conventions);
            _dsl = new ControllerActionDSL(_config, _conventions);

            _dsl.ByDefault.EveryControllerAction(d => d
                .Will<TestBehavior>()
                .Will<TestBehavior2>());
        }

        [Test]
        public void should_setup_action_with_default_behaviors()
        {
            _dsl.ForController<TestController>(x => x
                .Action<TestInputModel, TestOutputModel>(
                (c, i) => c.SomeAction(i)));

            _config.GetControllerActionConfigs().Single()
                .GetBehaviors().ShouldHaveTheSameElementsAs(typeof(TestBehavior), typeof(TestBehavior2));
        }

        [Test]
        public void should_support_behavior_overrides()
        {
            _dsl.ForController<TestController>(x => x
                .Action<TestInputModel, TestOutputModel>(
                (c, i) => c.SomeAction(i), o=>o.DoesNot<TestBehavior>()));

            _config.GetControllerActionConfigs().Single()
                .GetBehaviors().ShouldHaveTheSameElementsAs(typeof(TestBehavior2));
        }

        [Test]
        public void should_auto_config_controllers_according_to_conventions()
        {
            _dsl.AddControllersFromAssembly
                .ContainingType<TestController>(x => 
                    {
                        x.Where(t => t.Name == "TestController");
                        x.MapActionsWhere((m,i,o) => m.Name.Equals("SomeAction"));
                    });

            var actionConfig = _config.GetControllerActionConfigs().Single();
            
            actionConfig.ControllerType.ShouldEqual(typeof (TestController));
            actionConfig.ActionName.Equals("SomeAction", StringComparison.InvariantCultureIgnoreCase).ShouldBeTrue();
        }

        [Test]
        public void should_override_correct_action_config()
        {
            _dsl.AddControllersFromAssembly
                .ContainingType<TestController>(x =>
                {
                    x.Where(t => t.Name == "TestController");
                    x.MapActionsWhere((m, i, o) => m.Name.Equals("SomeAction"));
                });

            var actionConfig = _config.GetControllerActionConfigs().Single();

            var expectedUrl = "TESTURL";

            _dsl.OverrideConfigFor<TestController>(c=>c.SomeAction(null),
                config=>config.PrimaryUrl = expectedUrl);

            actionConfig.PrimaryUrl.ShouldEqual(expectedUrl);
        }
    }
}