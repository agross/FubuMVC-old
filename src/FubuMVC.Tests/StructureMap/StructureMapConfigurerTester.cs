using System.Collections.Generic;
using System.Linq;
using FubuMVC.Container.StructureMap.Config;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Controller.Config.DSL;
using FubuMVC.Core.Controller.Invokers;
using FubuMVC.Core.Conventions;
using NUnit.Framework;
using StructureMap.Configuration.DSL;

namespace FubuMVC.Tests.StructureMap
{
    [TestFixture]
    public class StructureMapConfigurerTester
    {
        private global::StructureMap.Container _container;
        private TestController _controller;
        private ThunderdomeActionInvoker<TestController, TestInputModel, TestOutputModel> _invoker;
        private ThunderdomeActionInvoker<TestController, TestInputModel, TestOutputModel2> _anotherInvoker;
        private ThunderdomeActionInvoker<TestController, TestInputModel, TestOutputModel3> _thirdActionInvoker;
        private FubuConfiguration _config;
        private FubuConventions _conventions;
        private StructureMapConfigurer _configurer;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            var registry = new Registry();
            _conventions = new FubuConventions();
            _config = new FubuConfiguration(_conventions);

            var dsl = new ControllerActionDSL(_config, _conventions);

            dsl.ByDefault.EveryControllerAction((d => d
                  .Will<TestBehavior>()
                  .Will<TestBehavior2>()));

            dsl.AddControllerActions(a =>
                 a.UsingTypesInTheSameAssemblyAs<TestController>(types =>
                      from t in types
                      where t.Name == "TestController"
                      from m in t.GetMethods()
                      where 
                        (m.Name == "SomeAction" && m.GetParameters()[0].ParameterType == typeof(TestInputModel))
                        || m.Name == "AnotherAction"
                        || m.Name == "ThirdAction"
                      select m));

            dsl.OverrideConfigFor<TestController>(c=>c.SomeAction(null), c=>c.RemoveAllBehaviors());
            dsl.OverrideConfigFor<TestController>(c => c.ThirdAction(null), c => c.RemoveBehavior<TestBehavior2>());

            dsl.Conventions.AddCustomConvention<TestControllerConvention, TestController>();
            dsl.Conventions.AddCustomConvention<AnotherTestControllerConvention, TestController>();
            dsl.Conventions.AddCustomConvention<TestBehaviorConvention, TestBehavior>();
            dsl.Conventions.AddCustomConvention<AnotherTestBehaviorConvention, TestBehavior>();

            _configurer = new StructureMapConfigurer(_conventions, _config);

            _configurer.ConfigureRegistry(registry);

            _container = new global::StructureMap.Container(x =>
            {
                x.AddRegistry(new FrameworkServicesRegistry());
                x.AddRegistry(registry);
            });

            _controller = _container.GetInstance<TestController>();

            var configs = _config.GetControllerActionConfigs().ToArray();

            var firstActionConfig = configs[0];
            var secondActionConfig = configs[1];
            var thirdActionConfig = configs[2];

            _invoker =
                _container.GetInstance<IControllerActionInvoker>(
                    firstActionConfig.UniqueID)
                    .ShouldBeOfType<ThunderdomeActionInvoker<TestController, TestInputModel, TestOutputModel>>();

            _anotherInvoker =
                _container.GetInstance<IControllerActionInvoker>(
                    secondActionConfig.UniqueID)
                    .ShouldBeOfType<ThunderdomeActionInvoker<TestController, TestInputModel, TestOutputModel2>>();

            _thirdActionInvoker =
                _container.GetInstance<IControllerActionInvoker>(
                    thirdActionConfig.UniqueID)
                    .ShouldBeOfType<ThunderdomeActionInvoker<TestController, TestInputModel, TestOutputModel3>>();
        }

        [Test]
        public void DSL_should_register_controller_properly()
        {
            _controller.ShouldNotBeNull();
        }

        [Test]
        public void DSL_should_register_action_invokers_properly()
        {
            _invoker
                .ShouldNotBeTheSameAs(_anotherInvoker)
                .ShouldNotBeTheSameAs(_thirdActionInvoker);

            _anotherInvoker
                .ShouldNotBeTheSameAs(_invoker)
                .ShouldNotBeTheSameAs(_thirdActionInvoker);

            _thirdActionInvoker
                .ShouldNotBeTheSameAs(_invoker)
                .ShouldNotBeTheSameAs(_anotherInvoker);
        }

        [Test]
        public void DSL_should_preserve_behavior_ordering()
        {
            _anotherInvoker
                .Behavior.ShouldBeOfType<TestBehavior>()
                .InsideBehavior.ShouldBeOfType<TestBehavior2>()
                .InsideBehavior.ShouldBeOfType<DefaultBehavior>();
        }

        [Test]
        public void DSL_should_allow_for_action_specific_decorator_ordering_and_including_or_exclusion()
        {
            _thirdActionInvoker.Behavior.ShouldBeOfType<TestBehavior>()
                .InsideBehavior.ShouldBeOfType<DefaultBehavior>();
        }

        [Test]
        public void DSL_should_register_custom_conventions_in_the_container()
        {
            var convArray = _container.GetAllInstances<IFubuConvention<TestBehavior>>().ToArray();

            convArray.ShouldHaveCount(2);
            convArray[0].ShouldBeOfType<TestBehaviorConvention>();
            convArray[1].ShouldBeOfType<AnotherTestBehaviorConvention>();
        }

        [Test]
        public void DSL_should_register_custom_conventions_in_the_container_and_inject_them_when_requested()
        {
            var convArray = _controller.Conventions.ToArray();

            convArray.ShouldHaveCount(2);
            convArray[0].ShouldBeOfType<TestControllerConvention>();
            convArray[1].ShouldBeOfType<AnotherTestControllerConvention>();
        }
    }
}