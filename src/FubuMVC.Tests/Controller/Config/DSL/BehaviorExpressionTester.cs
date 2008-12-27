using FubuMVC.Core.Controller.Config.DSL;
using NUnit.Framework;
using FubuMVC.Core.Controller.Config;

namespace FubuMVC.Tests.Controller.Config.DSL
{
    [TestFixture]
    public class BehaviorExpressionTester
    {
        [Test]
        public void should_remember_added_behavior_configuration_in_correct_order()
        {
            var config = new FubuConfiguration(null);
            new BehaviorExpression(config).Will<TestBehavior>().Will<TestBehavior2>();

            config.GetDefaultBehaviors().ShouldHaveTheSameElementsAs(typeof(TestBehavior), typeof(TestBehavior2));
        }
    }
}