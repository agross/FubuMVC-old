using FubuMVC.Core.Controller.Config.DSL;
using FubuMVC.Core.Util;
using NUnit.Framework;
using FubuMVC.Core.Controller.Config;

namespace FubuMVC.Tests.Controller.Config.DSL
{
    [TestFixture]
    public class ActionConfigExpressionTester
    {
        private ControllerActionConfig _config;
        private ActionConfigExpression _expression;

        [SetUp]
        public void SetUp()
        {
            var method = ReflectionHelper.GetMethod<TestController>(c => c.SomeAction(null));
            _config = new ControllerActionConfig(method, null, null);
            _expression = new ActionConfigExpression(_config);
        }

        [Test]
        public void AsUrl_should_set_the_primary_url()
        {
            var url = "foo";
            _expression.AsUrl(url);
            _config.PrimaryUrl.ShouldEqual(url);
        }

        [Test]
        public void AlsoAsUrl_should_add_an_alternate_url_to_the_config()
        {
            var url = "foo";
            _expression.AlsoAsUrl(url);
            _config.GetOtherUrls().ShouldHaveTheSameElementsAs(url);
        }

        [Test]
        public void Can_should_add_a_behavior_to_the_last_action_handler()
        {
            _expression.Will<TestBehavior>();

            _config.GetBehaviors().ShouldHaveTheSameElementsAs(typeof(TestBehavior));
        }

        [Test]
        public void RemoveAllBehaviors_should_clear_the_behaviors_for_that_action_handler()
        {
            _expression.Will<TestBehavior>();
            _expression.Will<TestBehavior2>();
            _expression.RemoveAllBehaviors();

            _config.GetBehaviors().ShouldHaveCount(0);
        }

        [Test]
        public void DoesNot_should_remove_that_behavior_for_that_action_handler()
        {
            _expression.Will<TestBehavior>();
            _expression.Will<TestBehavior2>();
            _expression.DoesNot<TestBehavior>();

            _config.GetBehaviors().ShouldHaveTheSameElementsAs(typeof(TestBehavior2));
        }
    }
}