using System;
using System.Linq;
using System.Reflection;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Controller.Config.DSL;
using NUnit.Framework;
using Rhino.Mocks;

namespace FubuMVC.Tests.Controller.Config.DSL
{
    [TestFixture]
    public class when_adding_custom_configurer
    {
        private AssemblyControllerScanningExpression _assemblyExpression;

        [SetUp]
        public void SetUp()
        {
            _assemblyExpression = new AssemblyControllerScanningExpression(null, null);
        }

        [Test]
        public void should_remember_custom_configurers()
        {
            var configurer = MockRepository.GenerateStub<IControllerActionConfigurer>();
            _assemblyExpression.UsingConfigurer( configurer );
            _assemblyExpression.CustomConfigurers.Single().ShouldBeTheSameAs(configurer);
        }
    }

    [TestFixture]
    public class when_using_custom_configurers
    {
        private AssemblyControllerScanningExpression _assemblyExpression;
        private IControllerActionConfigurer _configurer;
        private IControllerActionConfigurer _standardConfigurer;
        private FubuConventions _conventions;

        [SetUp]
        public void SetUp()
        {
            _conventions = new FubuConventions();
            _standardConfigurer = MockRepository.GenerateMock<IControllerActionConfigurer>();
            _configurer = MockRepository.GenerateMock<IControllerActionConfigurer>();

            _assemblyExpression = new AssemblyControllerScanningExpression(_conventions, new[]{_standardConfigurer});
            _assemblyExpression.UsingConfigurer(_configurer);
        }

        [Test]
        public void should_attempt_to_use_custom_configurers_first()
        {
            var action = typeof(TestController).GetMethod("AnotherAction");
            _configurer.Stub(c => c.ShouldConfigure(action)).Return(true);
            _configurer.Expect(c => c.Configure(null)).IgnoreArguments().Return(new ControllerActionConfig(action, null, null));

            _assemblyExpression.AddConfigFromDiscoveredAction(action);

            _configurer.AssertWasCalled(c => c.Configure(action));
        }

        [Test]
        public void should_use_standard_configurers_if_custom_cannot_configure_that_method()
        {
            var action = typeof(TestController).GetMethod("AnotherAction");
            _configurer.Stub(c => c.ShouldConfigure(action)).Return(false);
            _standardConfigurer.Stub(c => c.ShouldConfigure(action)).Return(true);
            _standardConfigurer.Stub(c => c.Configure(null)).IgnoreArguments().Return(new ControllerActionConfig(action, null, null));

            _assemblyExpression.AddConfigFromDiscoveredAction(action);

            _standardConfigurer.AssertWasCalled(c => c.Configure(action));
        }

        [Test]
        public void should_throw_if_no_configurer_can_configure_that_method()
        {
            var action = typeof(TestController).GetMethod("AnotherAction");
            _configurer.Stub(c => c.ShouldConfigure(action)).Return(false);
            _standardConfigurer.Stub(c => c.ShouldConfigure(action)).Return(false);
            
            typeof(InvalidOperationException).ShouldBeThrownBy(() =>
                _assemblyExpression.AddConfigFromDiscoveredAction(action)
            );
        }

    }

    [TestFixture]
    public class when_configuring_multiple_controller_actions_using_linq_selector
    {
        private AssemblyControllerScanningExpression _assemblyExpression;
        private IControllerActionConfigurer _standardConfigurer;
        private FubuConventions _conventions;
        private string _expectedUrl;

        [SetUp]
        public void SetUp()
        {
            _standardConfigurer = MockRepository.GenerateMock<IControllerActionConfigurer>();
            _conventions = new FubuConventions();
            _assemblyExpression = new AssemblyControllerScanningExpression(_conventions, new[] { _standardConfigurer });
            var action = typeof(TestController).GetMethod("AnotherAction");
            _standardConfigurer.Stub(c => c.ShouldConfigure(null)).IgnoreArguments().Return(true);
            _standardConfigurer.Stub(c => c.Configure(null)).IgnoreArguments().Return(new ControllerActionConfig(action, null, null));

            _expectedUrl = "EXPECTED_URL";
            _conventions.PrimaryUrlConvention = config => _expectedUrl;

            _assemblyExpression.UsingTypesInTheSameAssemblyAs<TestController>(types =>
                  from t in types
                  where t == typeof(TestController)
                  from m in t.GetMethods()
                  where m.Name.EndsWith("Action") && m.GetParameters()[0].ParameterType != typeof(Int32)
                  select m);
        }

        [Test]
        public void should_gather_action_configs()
        {
            _assemblyExpression.DiscoveredConfigs.Count().ShouldBeGreaterThan(3);
        }

        [Test]
        public void should_set_the_primary_url_based_on_conventions()
        {
            _assemblyExpression.DiscoveredConfigs.First().PrimaryUrl.ShouldEqual(_expectedUrl);
        }
    }

    [TestFixture]
    public class when_configuring_multiple_controller_actions_using_simple_selector
    {
        private AssemblyControllerScanningExpression _assemblyExpression;
        private IControllerActionConfigurer _standardConfigurer;
        private FubuConventions _conventions;
        private string _expectedUrl;

        [SetUp]
        public void SetUp()
        {
            _standardConfigurer = MockRepository.GenerateMock<IControllerActionConfigurer>();
            _conventions = new FubuConventions();
            _assemblyExpression = new AssemblyControllerScanningExpression(_conventions, new[] { _standardConfigurer });
            var action = typeof(TestController).GetMethod("AnotherAction");
            _standardConfigurer.Stub(c => c.ShouldConfigure(null)).IgnoreArguments().Return(true);
            _standardConfigurer.Stub(c => c.Configure(null)).IgnoreArguments().Return(new ControllerActionConfig(action, null, null));

            _expectedUrl = "EXPECTED_URL";
            _conventions.PrimaryUrlConvention = config => _expectedUrl;

            _assemblyExpression.UsingTypesInTheSameAssemblyAs<TestController>(x =>
                  x.SelectTypes(t => t == typeof (TestController))
                   .SelectMethods(m =>
                        m.Name.EndsWith("Action") &&
                        m.GetParameters()[0].ParameterType != typeof (Int32)));
        }

        [Test]
        public void should_gather_action_configs()
        {
            _assemblyExpression.DiscoveredConfigs.Count().ShouldBeGreaterThan(3);
        }

        [Test]
        public void should_set_the_primary_url_based_on_conventions()
        {
            _assemblyExpression.DiscoveredConfigs.First().PrimaryUrl.ShouldEqual(_expectedUrl);
        }
    }
}