using System.Web.Routing;
using FubuMVC.Core.Config;
using FubuMVC.Core.Runtime;
using NUnit.Framework;
using System.Linq;

namespace FubuMVC.Tests.Config
{
    [TestFixture]
    public class RouteConfigurerTester
    {
        private RouteConfigurer _registry;
        private FubuRouteHandler _handler;
        private FubuRouteHandler _otherHandler;

        [SetUp]
        public void SetUp()
        {
            var action = new UrlAction {PrimaryUrlStub = ""};
            action.AddOtherUrlStub("test");
            action.AddOtherUrlStub("test/someaction");

            _handler = new FubuRouteHandler(action, null, null, null);

            action = new UrlAction {PrimaryUrlStub = "test/anotheraction"};

            _otherHandler = new FubuRouteHandler(action, null, null, null);

            _registry = new RouteConfigurer(new[] { _handler, _otherHandler });
            _registry.Configure();
        }

        [Test]
        public void should_add_a_new_route_to_the_registered_route_list()
        {
            _registry.GetRegisteredRoutes().Count(r => r.RouteHandler == _handler).ShouldEqual(3);
        }

        [Test]
        public void should_register_index_url_for_controller()
        {
            _registry.GetRegisteredRoutes().ToArray()[1].Url.ShouldEqual("test");
        }

        [Test]
        public void should_register_index_url_for_controller_only_once()
        {
            _registry.GetRegisteredRoutes().Where(r=>r.Url.Equals("test")).ShouldHaveCount(1);
        }
        
        [Test]
        public void should_register_basic_url()
        {
            _registry.GetRegisteredRoutes().Skip(2).First().Url.ShouldEqual("test/someaction");
        }

        [Test]
        public void should_load_all_routes()
        {
            var col = new RouteCollection();
            _registry.LoadRoutes(col);
            col.ShouldHaveCount(4);
        }

        [Test]
        public void should_have_all_expected_route_urls()
        {
            _registry.GetRegisteredRoutes().Select(r => r.Url)
                .ShouldHaveTheSameElementsAs(
                "",
                "test",
                "test/someaction",
                "test/anotheraction");
        }
    }
}