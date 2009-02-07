using NUnit.Framework;
using FubuMVC.Core;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Html;

namespace FubuMVC.Tests.Controller.Config
{
    [TestFixture]
    public class UrlResolverTester
    {
        private FubuConfiguration _config;
        private FubuConventions _conventions;
        private string _root;
        private string _actionUrl;
        private string _controllerUrl;

        [SetUp]
        public void SetUp()
        {
            _root = "fake";
            _actionUrl = "test/someaction";
            _controllerUrl = "test";
            UrlContext.Stub(_root);

            _conventions = new FubuConventions();
            _config = new FubuConfiguration(_conventions);

            var actionConfig = ControllerActionConfig.ForAction<TestController, TestInputModel, TestOutputModel>(
                (c, i) => c.SomeAction(i), null);

            actionConfig.PrimaryUrl = _actionUrl;

            _config.AddControllerActionConfig(actionConfig);
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            UrlContext.Stub("");
        }

        [Test]
        public void should_resolve_to_full_url()
        {
            new UrlResolver(_config, _conventions).UrlFor<TestController>(c => c.SomeAction(null))
                .ShouldEqual("{0}/{1}".ToFormat(_root, _actionUrl));
        }

        [Test]
        public void should_resolve_controller_url_to_full_url()
        {
            new UrlResolver(_config, _conventions).UrlFor<TestController>()
                .ShouldEqual("{0}/{1}".ToFormat(_root, _controllerUrl));
        }
    }
}
