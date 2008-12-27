using System.Linq;
using FubuMVC.Container.StructureMap.Config;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Controller;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Controller.Config.DSL;
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

            dsl.ForController<TestController>(a => a
                .Action<TestInputModel, TestOutputModel>((c, i) => c.SomeAction(i), b => b.RemoveAllBehaviors())
                .Action<TestInputModel, TestOutputModel2>((c, i) => c.AnotherAction(i))
                .Action<TestInputModel, TestOutputModel3>((c, i) => c.ThirdAction(i), b => b.DoesNot<TestBehavior2>()));

            _configurer = new StructureMapConfigurer(_conventions, _config);

            _configurer.ConfigureRegistry(registry);

            _container = new global::StructureMap.Container(x =>
            {
                x.AddRegistry(new FrameworkServicesRegistry());
                x.AddRegistry(registry);
            });

            _controller = _container.GetInstance<TestController>();

            var configs = _config.GetControllerActionConfigs();

            var firstActionConfig = configs.First();
            var secondActionConfig = configs.Skip(1).First();
            var thirdActionConfig = configs.Skip(2).First();

            _invoker =
                _container.GetInstance<IControllerActionInvoker<TestController, TestInputModel, TestOutputModel>>(
                    firstActionConfig.UniqueID)
                    .ShouldBeOfType<ThunderdomeActionInvoker<TestController, TestInputModel, TestOutputModel>>();

            _anotherInvoker =
                _container.GetInstance<IControllerActionInvoker<TestController, TestInputModel, TestOutputModel2>>(
                    secondActionConfig.UniqueID)
                    .ShouldBeOfType<ThunderdomeActionInvoker<TestController, TestInputModel, TestOutputModel2>>();

            _thirdActionInvoker =
                _container.GetInstance<IControllerActionInvoker<TestController, TestInputModel, TestOutputModel3>>(
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
            _anotherInvoker.Behavior.ShouldBeOfType<TestBehavior2>()
                .InsideBehavior.ShouldBeOfType<TestBehavior>()
                .InsideBehavior.ShouldBeOfType<DefaultBehavior>();
        }

        [Test]
        public void DSL_should_allow_for_action_specific_decorator_ordering_and_including_or_exclusion()
        {
            _thirdActionInvoker.Behavior.ShouldBeOfType<TestBehavior>()
                .InsideBehavior.ShouldBeOfType<DefaultBehavior>();
        }
    }
}