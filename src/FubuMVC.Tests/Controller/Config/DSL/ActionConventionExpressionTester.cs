using System.Linq;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Controller.Config.DSL;
using FubuMVC.Core.Conventions;
using NUnit.Framework;

namespace FubuMVC.Tests.Controller.Config.DSL
{
    [TestFixture]
    public class ActionConventionExpressionTester
    {
        private ActionConventionExpression _expression;

        [SetUp]
        public void SetUp()
        {
            _expression = new ActionConventionExpression();
        }

        [Test]
        public void should_new_up_and_add_convention_to_list()
        {
            _expression.Add<PlainActionConv>();
            _expression.Conventions.ShouldHaveCount(1);
        }

        [Test]
        public void should_add_convention_instance_to_list()
        {
            var conv = new PlainActionConv();
            _expression.Add(conv);
            _expression.Conventions.First().ShouldBeTheSameAs(conv);
        }

        public class PlainActionConv : IFubuConvention<ControllerActionConfig>
        {
            public void Apply(ControllerActionConfig item)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}