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
        private wire_up_JSON_URL convention;
        private ControllerActionConfig config;

        [SetUp]
        public void SetUp()
        {
            expectedJsonExtension = "__EXPECTED_JSON__";
            fubuConventions = new FubuConventions { DefaultJsonExtension = expectedJsonExtension };
            convention = new wire_up_JSON_URL { FubuConventions = fubuConventions };

            var method = ReflectionHelper.GetMethod<TestController>(c => c.SomeAction(null));
            config = new ControllerActionConfig(method, null, null);
        }

        [Test]
        public void should_apply_url()
        {
            convention.Apply(config);

            config.GetOtherUrls().Single().EndsWith(expectedJsonExtension);
            
        }
    }
}