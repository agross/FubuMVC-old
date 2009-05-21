using System.Linq;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Controller.Config.DSL;
using NUnit.Framework;

namespace FubuMVC.Tests.Controller.Config.DSL
{
    [TestFixture]
    public class when_not_in_debug_mode : the_DebugOutputActionConfigurer_should
    {
        [Test]
        public void not_configure_debug_action()
        {
            DebugMode = false;

            ActionConfig.ShouldBeNull();
        }
    }

    [TestFixture]
    public class when_in_debug_mode : the_DebugOutputActionConfigurer_should
    {
        [Test]
        public void configure_the_debug_action()
        {
            ActionConfig.ShouldNotBeNull();
        }

        [Test]
        public void set_the_required_properties()
        {
            ActionConfig.PrimaryUrl.ShouldEqual(DebugOutputActionConfigurer.DEBUG_URL);
            ActionConfig.ActionName.ShouldEqual(DebugOutputActionConfigurer.DEBUG_URL);
            ActionConfig.ActionDelegate.ShouldNotBeNull();
            ActionConfig.ActionMethod.ShouldNotBeNull();
            ActionConfig.ControllerType.ShouldNotBeNull();
        }

        [Test]
        public void should_only_have_execute_result_and_output_debug_behaviors()
        {
            ActionConfig.GetBehaviors().First().ShouldEqual(typeof(execute_the_result));
            ActionConfig.GetBehaviors().Skip(1).First().ShouldEqual(typeof(OutputDebugInformation));
        }
    }

    public class the_DebugOutputActionConfigurer_should
    {
        protected FubuConventions _conv;
        protected FubuConfiguration _config;
        protected DebugOutputActionConfigurer _configurer;
        protected ControllerActionConfig _actionConfig;

        [SetUp]
        public void SetUp()
        {
            _conv = new FubuConventions();
            DebugMode = true;
            _config = new FubuConfiguration(_conv);
            _configurer = new DebugOutputActionConfigurer(_conv, _config);
        }

        public bool DebugMode
        {
            get { return _conv.DebugMode(); }
            set { _conv.DebugMode = () => value; }
        }

        public ControllerActionConfig ActionConfig
        {
            get
            {
                return _actionConfig = _actionConfig ?? getActionConfig();
            }
        }

        protected ControllerActionConfig getActionConfig()
        {
            _configurer.Configure();
            return _config.GetControllerActionConfigs().SingleOrDefault();
        }
    }
}