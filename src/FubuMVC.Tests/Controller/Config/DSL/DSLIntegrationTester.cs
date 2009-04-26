using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        public void should_setup_actions_when_using_simple_action_selector()
        {
            _dsl.AddControllerActions(a =>
                 a.UsingTypesInTheSameAssemblyAs<TestController>(x =>
                     x.SelectTypes(t=> t == typeof(TestController))
                      .SelectMethods(m=>
                      m.Name == "SomeAction" && m.GetParameters()[0].ParameterType == typeof(TestInputModel)
                      )));

            _config.GetControllerActionConfigs().Single()
                .GetBehaviors().ShouldHaveTheSameElementsAs(typeof(TestBehavior), typeof(TestBehavior2));
        }

        [Test]
        public void should_setup_action_with_default_behaviors()
        {
            _dsl.AddControllerActions(a =>
                 a.UsingTypesInTheSameAssemblyAs<TestController>(types =>
                      from t in types
                      where t.Name == "TestController"
                      from m in t.GetMethods()
                      where m.Name == "SomeAction" && m.GetParameters()[0].ParameterType == typeof(TestInputModel)
                      select m));

            _config.GetControllerActionConfigs().Single()
                .GetBehaviors().ShouldHaveTheSameElementsAs(typeof(TestBehavior), typeof(TestBehavior2));
        }

        [Test]
        public void should_support_behavior_overrides()
        {
            _dsl.AddControllerActions(a =>
                 a.UsingTypesInTheSameAssemblyAs<TestController>(types =>
                      from t in types
                      where t.Name == "TestController"
                      from m in t.GetMethods()
                      where m.Name == "SomeAction" && m.GetParameters()[0].ParameterType == typeof(TestInputModel)
                      select m));

            _dsl.OverrideConfigFor<TestController>(c=>c.SomeAction(null),config=>config.RemoveBehavior<TestBehavior>());

            _config.GetControllerActionConfigs().Single()
                .GetBehaviors().ShouldHaveTheSameElementsAs(typeof(TestBehavior2));
        }

        [Test]
        public void should_auto_config_controllers_according_to_conventions()
        {
            _dsl.AddControllerActions(a =>
                  a.UsingTypesInTheSameAssemblyAs<TestController>(types =>
                       from t in types
                       where t.Name == "TestController"
                       from m in t.GetMethods()
                       where m.Name == "SomeAction" && m.GetParameters()[0].ParameterType == typeof(TestInputModel)
                       select m));
            
            var actionConfig = _config.GetControllerActionConfigs().Single();
            
            actionConfig.ControllerType.ShouldEqual(typeof (TestController));
            actionConfig.ActionName.Equals("SomeAction", StringComparison.InvariantCultureIgnoreCase).ShouldBeTrue();
        }

        [Test]
        public void should_override_correct_action_config()
        {
            _dsl.AddControllerActions(a =>
                  a.UsingTypesInTheSameAssemblyAs<TestController>(types =>
                       from t in types
                       where t.Name == "TestController"
                       from m in t.GetMethods()
                       where m.Name == "SomeAction" && m.GetParameters()[0].ParameterType == typeof(TestInputModel)
                       select m));

            var actionConfig = _config.GetControllerActionConfigs().Single();

            var expectedUrl = "TESTURL";

            _dsl.OverrideConfigFor<TestController>(c=>c.SomeAction(null),
                config=>config.PrimaryUrl = expectedUrl);

            actionConfig.PrimaryUrl.ShouldEqual(expectedUrl);
        }

        [Test]
        public void should_override_correct_actionname_from_an_other_action()
        {
            _dsl.AddControllerActions(a =>
                  a.UsingTypesInTheSameAssemblyAs<TestController>(types =>
                       from t in types
                       where t.Name == "TestController"
                       from m in t.GetMethods()
                       where m.Name.EndsWith("Action") &&  m.GetParameters()[0].ParameterType != typeof(Int32)
                       select m)); 

            var actionConfig = _config.GetControllerActionConfigs().Where(c => c.PrimaryUrl == "test/someaction").FirstOrDefault();

            var expectedActionName = "anotheraction";

            Expression<Func<TestController, object>> AnotherAction = c => c.AnotherAction(null);

            _dsl.OverrideConfigFor<TestController>(c => c.SomeAction(null),
                config => config.UseViewFrom(AnotherAction));

            actionConfig.ActionName.ShouldEqual(expectedActionName);
        }

        [Test]
        public void should_add_action_conventions_to_fubuconfiguration()
        {
            _dsl.ActionConventions(c => c.Add<UrlHappyConventionForTestPurposes>());

            _config.GetActionConventions().First().ShouldBeOfType<UrlHappyConventionForTestPurposes>();
        }
    }
}