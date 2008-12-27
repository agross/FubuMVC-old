using System.Web.Routing;
using NUnit.Framework;
using FubuMVC.Core.Controller.Config;
using System.Linq;

namespace FubuMVC.Tests.Controller.Config
{
    [TestFixture]
    public class RouteConfigurerTester
    {
        private RouteConfigurer _registry;
        private ControllerActionConfig _config;
        private FubuConventions _conventions;
        private FubuConfiguration _fubuConfig;

        [SetUp]
        public void SetUp()
        {
            _config = ControllerActionConfig.ForAction<TestController, TestInputModel, TestOutputModel>(
                (c, i) => c.SomeAction(i));

            _config.PrimaryUrl = "test/someaction";
            
            _conventions = new FubuConventions();
            _fubuConfig = new FubuConfiguration(_conventions);
            _fubuConfig.AddControllerActionConfig(_config);
            _registry = new RouteConfigurer(_fubuConfig, _conventions);
        }

        [Test]
        public void should_add_a_new_route_to_the_registered_route_list()
        {
            _registry.GetRegisteredRoutes().Count(r => ((ActionRouteHandler) r.RouteHandler).Config == _config).ShouldEqual(3);
        }

        [Test]
        public void should_register_index_url_for_controller()
        {
            _registry.GetRegisteredRoutes().Skip(1).First().Url.ShouldEqual("test");
        }
        
        [Test]
        public void should_register_basic_url()
        {
            _registry.GetRegisteredRoutes().Skip(2).First().Url.ShouldEqual("test/someaction");
        }
        
        [Test]
        public void should_make_first_route_the_app_default()
        {
            ((ActionRouteHandler)_registry.AppDefaultRoute.RouteHandler).Config.ShouldBeTheSameAs(_config);
        }

        [Test]
        public void should_override_app_default_if_specified()
        {
            _config = ControllerActionConfig.ForAction<TestController, TestInputModel, TestOutputModel>(
                (c, i) => c.SomeAction(i));

            _conventions.IsAppDefaultUrl = config => true;

            _fubuConfig = new FubuConfiguration(_conventions);
            _fubuConfig.AddControllerActionConfig(_config);

            _registry = new RouteConfigurer(_fubuConfig, _conventions);

            ((ActionRouteHandler)_registry.AppDefaultRoute.RouteHandler).Config.ShouldBeTheSameAs(_config);
        }
     
        [Test]
        public void the_first_action_should_be_the_system_default()
        {
            _config = ControllerActionConfig.ForAction<TestController, TestInputModel, TestOutputModel>(
                (c, i) => c.SomeAction(i));

            _fubuConfig = new FubuConfiguration(_conventions);
            _fubuConfig.AddControllerActionConfig(_config);

            _registry = new RouteConfigurer(_fubuConfig, _conventions);

            ((ActionRouteHandler)_registry.AppDefaultRoute.RouteHandler).Config.ShouldBeTheSameAs(_config);
        }

        [Test]
        public void should_load_all_routes()
        {
            var col = new RouteCollection();
            _registry.LoadRoutes(col);
            col.ShouldHaveCount(3);
        }
    }
}
