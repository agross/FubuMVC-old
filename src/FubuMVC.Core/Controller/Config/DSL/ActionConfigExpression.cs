using FubuMVC.Core.Behaviors;

namespace FubuMVC.Core.Controller.Config.DSL
{
    public class ActionConfigExpression
    {
        public ActionConfigExpression(ControllerActionConfig config)
        {
            Config = config;
        }

        public ControllerActionConfig Config { get; private set; }

        public ActionConfigExpression AsUrl(string alias)
        {
            Config.PrimaryUrl = alias;
            return this;
        }

        public ActionConfigExpression AlsoAsUrl(string url)
        {
            Config.AddOtherUrl(url);
            return this;
        }

        public ActionConfigExpression Will<BEHAVIOR>() where BEHAVIOR : IControllerActionBehavior
        {
            Config.AddBehavior<BEHAVIOR>();
            return this;
        }

        public ActionConfigExpression RemoveAllBehaviors()
        {
            Config.RemoveAllBehaviors();
            return this;
        }

        public ActionConfigExpression DoesNot<BEHAVIOR>() where BEHAVIOR : IControllerActionBehavior
        {
            Config.RemoveBehavior<BEHAVIOR>();
            return this;
        }
    }
}