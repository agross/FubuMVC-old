using System;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Conventions.ControllerActions;
using FubuMVC.Core.Util;
using NUnit.Framework;

namespace FubuMVC.Tests.Conventions.ControllerActions
{
    [TestFixture]
    public class Convention_404_Handler
    {
        private string expectedPrimaryUrl;
        private FubuConventions fubuConventions;
        private wire_up_404_handler_URL convention;

        [SetUp]
        public void SetUp()
        {
            expectedPrimaryUrl = "__EXPECTED_404__";
            fubuConventions = new FubuConventions {PageNotFoundUrl = expectedPrimaryUrl};
            convention = new wire_up_404_handler_URL {FubuConventions = fubuConventions};
        }

        [Test]
        public void should_not_apply_if_not_PageNotFound_controller_Index_action()
        {
            var method = ReflectionHelper.GetMethod<TestController>(c => c.SomeAction(null));
            var config = new ControllerActionConfig(method, null, null);

            expectedPrimaryUrl = config.PrimaryUrl;

            convention.Apply(config);

            config.PrimaryUrl.ShouldEqual(expectedPrimaryUrl);
        }

        [Test]
        public void should_only_apply_if_PageNotFound_controller_Index_action()
        {
            var method = ReflectionHelper.GetMethod<PageNotFoundController>(c => c.Index(null));
            var config = new ControllerActionConfig(method, null, null);

            convention.Apply(config);

            config.PrimaryUrl.ShouldEqual(expectedPrimaryUrl);
        }

        private class PageNotFoundController
        {
            public object Index(object input)
            {
                throw new NotImplementedException();
            }
        }
    }
}