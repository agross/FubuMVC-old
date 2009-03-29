using System.Linq;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Conventions.ControllerActions;
using FubuMVC.Core.Util;
using NUnit.Framework;

namespace FubuMVC.Tests.Conventions.ControllerActions
{
    [TestFixture]
    public class Convention_JSON_URL_Handler
    {
        private string expectedJsonExtension;
        private FubuConventions fubuConventions;
        private wire_up_JSON_URL_if_required convention;
        private ControllerActionConfig config;

        [SetUp]
        public void SetUp()
        {
            expectedJsonExtension = "__EXPECTED_JSON__";
            fubuConventions = new FubuConventions { DefaultJsonExtension = expectedJsonExtension };
            convention = new wire_up_JSON_URL_if_required(fubuConventions);

            var method = ReflectionHelper.GetMethod<TestController>(c => c.SomeAction(null));
            config = new ControllerActionConfig(method, null, null);
        }

        [Test]
        public void should_not_apply_if_not_OutputAsJsonBehavior_not_defined()
        {
            convention.Apply(config);

            config.GetOtherUrls().ShouldHaveCount(0);
        }

        [Test]
        public void should_only_apply_if_OutputAsJsonBehavior_defined()
        {
            config.AddBehavior<OutputAsJson>();

            convention.Apply(config);

            config.GetOtherUrls().Single().EndsWith(expectedJsonExtension);
            
        }
    }
}