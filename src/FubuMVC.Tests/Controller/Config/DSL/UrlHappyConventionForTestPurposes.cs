using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Conventions;

namespace FubuMVC.Tests.Controller.Config.DSL
{
    public class UrlHappyConventionForTestPurposes : IFubuConvention<ControllerActionConfig>
    {
        public void Apply(ControllerActionConfig item)
        {
            item.PrimaryUrl = "HAPPY";
        }
    }
}