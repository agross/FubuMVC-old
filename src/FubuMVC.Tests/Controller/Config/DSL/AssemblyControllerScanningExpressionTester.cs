using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Controller.Config.DSL;
using NUnit.Framework;

namespace FubuMVC.Tests.Controller.Config.DSL
{
    [TestFixture]
    public class AssemblyControllerScanningExpressionTester
    {
        [Test]
        public void ContainingType_should_set_up_a_type_scanning_expression_with_the_correct_assembly()
        {
            ControllerTypeScanningExpression expression = null;
            new AssemblyControllerScanningExpression(null, null).ContainingType<TestController>(x => { expression = x; });

            expression.Assembly.ShouldEqual(typeof (TestController).Assembly);
        }

        [Test]
        public void should_apply_auto_config_to_overall_config()
        {
            var conventions = new FubuConventions();
            var fubuConfig = new FubuConfiguration(conventions);
            new AssemblyControllerScanningExpression(fubuConfig, conventions).ContainingType<TestController>(x =>
            {
                x.Where(t => t.Equals(typeof (TestController)));
                x.MapActionsWhere((m,i,o)=>m.Name.Equals("SomeAction"));
            });

            fubuConfig.GetConfigForAction<TestController>(c => c.SomeAction(null)).ShouldNotBeNull();
        }
    }
}