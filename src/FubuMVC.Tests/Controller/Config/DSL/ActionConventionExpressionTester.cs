using System.Linq;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Controller.Config.DSL;
using FubuMVC.Core.Conventions;
using FubuMVC.Core.Conventions.ControllerActions;
using NUnit.Framework;

namespace FubuMVC.Tests.Controller.Config.DSL
{
    [TestFixture]
    public class ActionConventionExpressionTester
    {
        private ActionConventionExpression _expression;
        private FubuConventions _fubuConventions;

        [SetUp]
        public void SetUp()
        {
            _fubuConventions = new FubuConventions();
            _expression = new ActionConventionExpression(_fubuConventions);
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

        [Test]
        public void should_set_fubuconventions_property_if_IControllerActionConfigConvention()
        {
            _expression.Add<FancyActionConv>();
            _expression.Conventions
                .First().ShouldBeOfType<FancyActionConv>()
                .FubuConventions.ShouldBeTheSameAs(_fubuConventions);
        }

        public class PlainActionConv : IFubuConvention<ControllerActionConfig>
        {
            public void Apply(ControllerActionConfig item)
            {
                throw new System.NotImplementedException();
            }
        }

        public class FancyActionConv : IControllerActionConfigConvention
        {
            public void Apply(ControllerActionConfig item)
            {
                throw new System.NotImplementedException();
            }

            public FubuConventions FubuConventions{ get; set; }
        }
    }
}