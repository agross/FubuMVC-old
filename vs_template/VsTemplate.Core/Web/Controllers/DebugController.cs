using System.Collections.Generic;
using VsTemplate.Core.Web.DisplayModels;
using FubuMVC.Core;
using FubuMVC.Core.Controller.Config;

namespace VsTemplate.Core.Web.Controllers
{
    public class DebugController
    {
        private readonly FubuConventions _conventions;
        private readonly FubuConfiguration _configuration;

        public DebugController(FubuConventions conventions, FubuConfiguration configuration)
        {
            _conventions = conventions;
            _configuration = configuration;
        }

        public DebugViewModel Index(DebugViewModel inModel)
        {
            var controllerActions = _configuration.GetControllerActionConfigs();
            List<ControllerActionDisplay> controllers = new List<ControllerActionDisplay>();
            controllerActions.Each(c => controllers.Add(new ControllerActionDisplay(c)));

            return new DebugViewModel
            {
                Controllers = controllers,
                ViewFileBasePath = _conventions.ViewFileBasePath,
                LayoutViewFileBasePath = _conventions.LayoutViewFileBasePath,
                SharedViewFileBasePath = _conventions.SharedViewFileBasePath,
            };
        }
    }

    public class DebugViewModel : ViewModel
    {
        public IList<ControllerActionDisplay> Controllers { get; set; }

        public string ViewFileBasePath { get; set; }
        public string LayoutViewFileBasePath { get; set; }
        public string SharedViewFileBasePath { get; set; }
        //public string DefaultPathToViewForAction { get; set; }
        //public string UrlRouteParametersForAction { get; set; }
        //public string DefaultPathToPartialView { get; set; }
        //public string CanonicalControllerName { get; set; }
        //public string PrimaryUrlConvention { get; set; }
        //public string DefaultUrlForController { get; set; }
        //public string PartialForEachOfHeader { get; set; }
        //public string PartialForEachOfBeforeEachItem { get; set; }
        //public string PartialForEachOfAfterEachItem { get; set; }
        //public string PartialForEachOfFooter { get; set; }
    }
}