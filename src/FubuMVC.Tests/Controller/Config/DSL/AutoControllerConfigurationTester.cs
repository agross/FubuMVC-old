using System.Linq;
using FubuMVC.Core.Util;
using NUnit.Framework;
using FubuMVC.Core.Controller.Config.DSL;

namespace FubuMVC.Tests.Controller.Config.DSL
{
    [TestFixture]
    public class AutoControllerConfigurationTester
    {
        [Test]
        public void should_add_discovered_type_to_collection()
        {
            var config = new AutoControllerConfiguration();
            config.AddDiscoveredType(typeof (TestController));
            config.GetDiscoveredTypes().Single().ShouldEqual(typeof (TestController));
        }

        [Test]
        public void should_not_add_duplicate_discovered_types_to_collection()
        {
            var config = new AutoControllerConfiguration();
            config.AddDiscoveredType(typeof(TestController));
            config.AddDiscoveredType(typeof(TestController));
            config.GetDiscoveredTypes().Single().ShouldEqual(typeof(TestController));
        }

        [Test]
        public void should_add_discovered_action_to_collection()
        {
            var config = new AutoControllerConfiguration();
            var action = new DiscovererdAction
                {
                    ControllerType = typeof (TestController),
                    Action = ReflectionHelper.GetMethod<TestController>(c => c.SomeAction(null))
                };
            config.AddDiscoveredAction(action);
            config.GetDiscoveredActions().Single().Action.Name.ShouldEqual("SomeAction");
        }

        [Test]
        public void should_not_add_duplicate_discovered_actions_to_collection()
        {
            var config = new AutoControllerConfiguration();
            var action1 = new DiscovererdAction
            {
                ControllerType = typeof(TestController),
                Action = ReflectionHelper.GetMethod<TestController>(c => c.SomeAction(null))
            };

            var action2 = new DiscovererdAction
            {
                ControllerType = typeof(TestController),
                Action = ReflectionHelper.GetMethod<TestController>(c => c.SomeAction(null))
            };
            config.AddDiscoveredAction(action1);
            config.AddDiscoveredAction(action2);
            config.GetDiscoveredActions().Single().Action.Name.ShouldEqual("SomeAction");
        }
    }
}