using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FubuMVC.Core.Conventions;
using FubuMVC.Core.Html.Expressions;
using FubuMVC.Core.Util;

namespace FubuMVC.Core.Controller.Config
{
    //public class FubuConventionSet<T>
    //    where T : class
    //{
    //    public Type ForType { get; private set; }
    //    public Type[] ConventionTypes { get; private set; }
    //}

    public class FubuConventions
    {
        private readonly Cache<Type,IList<Type>> _customConventions = new Cache<Type, IList<Type>>(t=>new List<Type>());

        public FubuConventions()
        {
            SetBaseDefaults();
        }

        public string ViewFileBasePath { get; set; }
        public string LayoutViewFileBasePath { get; set; }
        public string SharedViewFileBasePath { get; set; }
        public string DefaultRssExtension { get; set; }
        public string DefaultAtomExtension { get; set; }
        public string DefaultJsonExtension { get; set; }
        public string PrimaryApplicationUrl { get; set; }
        public string PageNotFoundUrl { get; set; }
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

            PrimaryApplicationUrl = "home/index";
            PageNotFoundUrl = "404";

            PrimaryUrlConvention = config => "{0}/{1}{2}".ToFormat(
                                                 CanonicalControllerName(config.ControllerType),
                                                 config.ActionName,
                                                 UrlRouteParametersForAction(config));

            DefaultUrlForController = CanonicalControllerName;

            IsAppDefaultUrl = config => PrimaryUrlConvention(config) == PrimaryApplicationUrl;
            
            ViewFileBasePath = "~/Views";
            LayoutViewFileBasePath = "~/Views/Layouts";
            SharedViewFileBasePath = "~/Views/Shared";

            DefaultRssExtension = ".rss";
            DefaultAtomExtension = ".atom";
            DefaultJsonExtension = ".json";

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

        public virtual void AddCustomConvention<CONVENTION,TARGET>()
            where CONVENTION : IFubuConvention<TARGET>
            where TARGET : class
        {
            _customConventions.Retrieve(typeof (TARGET)).Add(typeof (CONVENTION));
        }

        public IEnumerable<Type> GetCustomConventionTargetTypes()
        {
            return _customConventions.GetAllKeys();
        }

        public IEnumerable<Type> GetCustomConventionTypesFor(Type targetType)
        {
            return _customConventions.Retrieve(targetType);
        }
    }
}
