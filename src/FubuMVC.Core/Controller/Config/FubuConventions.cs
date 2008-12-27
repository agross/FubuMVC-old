using System;

namespace FubuMVC.Core.Controller.Config
{
    public class FubuConventions
    {
        public FubuConventions()
        {
            SetBaseDefaults();
        }

        public string ViewFileBasePath { get; set; }
        public Func<ControllerActionConfig, string> DefaultPathToViewForAction { get; set; }
        public Func<Type, string> CanonicalControllerName { get; set; }
        public Func<ControllerActionConfig, string> PrimaryUrlConvention { get; set; }
        public Func<Type, string> DefaultUrlForController { get; set; }
        public Func<ControllerActionConfig, bool> IsAppDefaultUrl { get; set; }

        protected void SetBaseDefaults()
        {
            CanonicalControllerName = controllerType => controllerType.Name.ToLowerInvariant().Replace("controller", "");

            PrimaryUrlConvention = config => "{0}/{1}".ToFormat(
                                                 CanonicalControllerName(config.ControllerType),
                                                 config.ActionName);

            DefaultUrlForController = CanonicalControllerName;

            IsAppDefaultUrl = config => PrimaryUrlConvention(config) == "home/index";


            ViewFileBasePath = "~/Views";

            DefaultPathToViewForAction = config =>
            {
                var controllerName = CanonicalControllerName(config.ControllerType);
                var actionName = config.ActionName;
                return "{0}/{1}/{2}.aspx".ToFormat(ViewFileBasePath, controllerName, actionName);
            };

        }

        public virtual void SetDefaults()
        {
            SetBaseDefaults();
        }

        public virtual string GetCanonicalControllerName<CONTROLLER>()
        {
            return CanonicalControllerName(typeof (CONTROLLER));
        }

        
    }
}