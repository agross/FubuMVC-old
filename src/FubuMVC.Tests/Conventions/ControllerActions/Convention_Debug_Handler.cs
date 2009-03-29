using System;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Conventions.ControllerActions;
using FubuMVC.Core.Util;
using NUnit.Framework;

namespace FubuMVC.Tests.Conventions.ControllerActions
{
    [TestFixture]
    public class Convention_Debug_Handler
    {
        private wire_up_debug_handler_URL convention;

        [SetUp]
        public void SetUp()
        {
            convention = new wire_up_debug_handler_URL();
        }

        [Test]
        public void should_not_apply_if_not_Debug_controller_Index_action()
        {
            var method = ReflectionHelper.GetMethod<TestController>(c => c.SomeAction(null));
            var config = new ControllerActionConfig(method, null, null);

            var expectedPrimaryUrl = config.PrimaryUrl;

            convention.Apply(config);

            config.PrimaryUrl.ShouldEqual(expectedPrimaryUrl);
        }

        [Test]
        public void should_only_apply_if_Debug_controller_Index_action()
        {
            var method = ReflectionHelper.GetMethod<DebugController>(c => c.Index(null));
            var config = new ControllerActionConfig(method, null, null);

            convention.Apply(config);

            config.PrimaryUrl.ShouldEqual(wire_up_debug_handler_URL.DEBUG_URL);
        }

        private class DebugController
        {
            public object Index(object input)
            {
                throw new NotImplementedException();
            }
        }
    }
}