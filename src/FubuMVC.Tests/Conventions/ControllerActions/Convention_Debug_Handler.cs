//using System.Linq;
//using FubuMVC.Core.Behaviors;
//using FubuMVC.Core.Controller.Config;
//using FubuMVC.Core.Conventions.ControllerActions;
//using FubuMVC.Core.Util;
//using NUnit.Framework;

//namespace FubuMVC.Tests.Conventions.ControllerActions
//{
//    [TestFixture]
//    public class Convention_Debug_Handler
//    {
//        private wire_up_debug_handler_URL convention;

//        [SetUp]
//        public void SetUp()
//        {
//            convention = new wire_up_debug_handler_URL();
//        }

//        [Test]
//        public void Should_add_the_debug_url_to_the_controller_that_is_equal_to_the_primary_application_url()
//        {
//            FubuConventions fubuConventions = new FubuConventions {PrimaryApplicationUrl = "test/index"};

//            var method = ReflectionHelper.GetMethod<TestController>(c => c.SomeAction(null));
//            var config = new ControllerActionConfig(method, null, null) {PrimaryUrl = "test/index"};
//            config.AddBehavior<OutputDebugInformation>();

//            convention.FubuConventions = fubuConventions;
//            convention.Apply(config);

//            config.GetOtherUrls().Contains(wire_up_debug_handler_URL.DEBUG_URL).ShouldBeTrue();
//        }

//        [Test]
//        public void Should_not_add_the_debug_url_to_the_controller_that_is_not_equal_to_the_primary_application_url()
//        {
//            FubuConventions fubuConventions = new FubuConventions {PrimaryApplicationUrl = "test123/index"};

//            var method = ReflectionHelper.GetMethod<TestController>(c => c.SomeAction(null));
//            var config = new ControllerActionConfig(method, null, null) {PrimaryUrl = "test/index"};
//            config.AddBehavior<OutputDebugInformation>();

//            convention.FubuConventions = fubuConventions;
//            convention.Apply(config);

//            config.GetOtherUrls().Contains(wire_up_debug_handler_URL.DEBUG_URL).ShouldBeFalse();
//        }

//        [Test]
//        public void Should_not_add_the_debug_url_if_the_debug_behavior_is_not_added_to_the_controller()
//        {
//            FubuConventions fubuConventions = new FubuConventions { PrimaryApplicationUrl = "test/index" };

//            var method = ReflectionHelper.GetMethod<TestController>(c => c.SomeAction(null));
//            var config = new ControllerActionConfig(method, null, null) { PrimaryUrl = "test/index" };

//            convention.FubuConventions = fubuConventions;
//            convention.Apply(config);

//            config.GetOtherUrls().Contains(wire_up_debug_handler_URL.DEBUG_URL).ShouldBeFalse();
//        }
//    }
//}