using System;
using System.Linq;
using System.Reflection;
using System.Text;
using FubuMVC.Core.Html.Expressions;

namespace FubuMVC.Core.Controller.Config
{
    public class FubuConventions
    {
        public FubuConventions()
        {
            SetBaseDefaults();
        }

        public string ViewFileBasePath { get; set; }
        public string LayoutViewFileBasePath { get; set; }
        public string SharedViewFileBasePath { get; set; }
        public Func<ControllerActionConfig, string> DefaultPathToViewForAction { get; set; }
        public Func<ControllerActionConfig, string> UrlRouteParametersForAction { get; set; }
        public Func<Type, string> DefaultPathToPartialView { get; set; }
        public Func<Type, string> CanonicalControllerName { get; set; }
        public Func<ControllerActionConfig, string> PrimaryUrlConvention { get; set; }
        public Func<Type, string> DefaultUrlForController { get; set; }
        public Func<ControllerActionConfig, bool> IsAppDefaultUrl { get; set; }
        public Func<object, int, HtmlExpressionBase> PartialForEachOfHeader { get; set; }
        public Func<object, int, int, HtmlExpressionBase> PartialForEachOfBeforeEachItem { get; set; }
        public Func<object, int, int, string> PartialForEachOfAfterEachItem { get; set; }
        public Func<object, int, string> PartialForEachOfFooter { get; set; }
        

        protected void SetBaseDefaults()
        {
            
            CanonicalControllerName = controllerType => controllerType.Name.ToLowerInvariant().Replace("controller", "");

            UrlRouteParametersForAction = GetUrlRouteParameters;

            PrimaryUrlConvention = config => "{0}/{1}{2}".ToFormat(
                                                 CanonicalControllerName(config.ControllerType),
                                                 config.ActionName,
                                                 UrlRouteParametersForAction(config));

            DefaultUrlForController = CanonicalControllerName;

            IsAppDefaultUrl = config => PrimaryUrlConvention(config) == "home/index";
            
            ViewFileBasePath = "~/Views";
            LayoutViewFileBasePath = "~/Views/Layouts";
            SharedViewFileBasePath = "~/Views/Shared";

            DefaultPathToViewForAction = config =>
            {
                var controllerName = CanonicalControllerName(config.ControllerType);
                var actionName = config.ActionName;
                return "{0}/{1}/{2}.aspx".ToFormat(ViewFileBasePath, controllerName, actionName);
            };

            DefaultPathToPartialView = viewType => "{0}/{1}.ascx".ToFormat(SharedViewFileBasePath, viewType.Name);

            PartialForEachOfHeader = (model, totalCount) => new GenericOpenTagExpression("ul");
            PartialForEachOfBeforeEachItem = (model, index, total) => new GenericOpenTagExpression("li");
            PartialForEachOfAfterEachItem = (model, index, total) => "</li>";
            PartialForEachOfFooter = (model, totalCount) => "</ul>";

        }

        public virtual void SetDefaults()
        {
            SetBaseDefaults();
        }

        public virtual string GetCanonicalControllerName<CONTROLLER>()
        {
            return CanonicalControllerName(typeof (CONTROLLER));
        }

        public virtual string GetUrlRouteParameters(ControllerActionConfig config)
        {
            var requiredProps =
                config.InputType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(p => p.HasCustomAttribute<RequiredAttribute>());

            var builder = new StringBuilder();

            requiredProps.Each(p => builder.AppendFormat("/{{{0}}}", p.Name));

            return builder.ToString();
        }
    }
}
