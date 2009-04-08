using System.Linq;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Controller.Config;

namespace FubuMVC.Core.Conventions.ControllerActions
{
    public class wire_up_RSS_and_ATOM_URLs_if_required : IControllerActionConfigConvention
    {
        public void Apply(ControllerActionConfig actionConfig)
        {
            if(! actionConfig.GetBehaviors().Any(b=>b == typeof(OutputAsRssOrAtomFeed))) return;

            actionConfig.AddOtherUrl(actionConfig.PrimaryUrl + FubuConventions.DefaultRssExtension);
            actionConfig.AddOtherUrl(actionConfig.PrimaryUrl + FubuConventions.DefaultAtomExtension);
        }

        public FubuConventions FubuConventions { get; set; }
    }
}