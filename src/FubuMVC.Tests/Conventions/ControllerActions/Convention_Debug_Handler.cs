using System;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Conventions.ControllerActions;
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
            var config =
                ControllerActionConfig.ForAction<TestController, TestInputModel, TestOutputModel>(
                    (c, i) => c.SomeAction(i));

            var expectedPrimaryUrl = config.PrimaryUrl;

            convention.Apply(config);

            config.PrimaryUrl.ShouldEqual(expectedPrimaryUrl);
        }

        [Test]
        public void should_only_apply_if_Debug_controller_Index_action()
        {
            var config =
                ControllerActionConfig.ForAction<DebugController, object, object>(
                    (c, i) => c.Index(i));

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