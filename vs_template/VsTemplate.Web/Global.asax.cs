using System;
using System.Linq.Expressions;
using System.Web;
using FubuMVC.Core.Html.Expressions;
using VsTemplate.Core.Web;
using VsTemplate.Core.Web.Behaviors;
using VsTemplate.Core.Web.Controllers;
using FubuMVC.Container.StructureMap.Config;
using FubuMVC.Core.Behaviors;

namespace VsTemplate.Web
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            ControllerConfig.Configure = x =>
            {
                x.UsingConventions(conventions =>
                {
                    conventions.PartialForEachOfHeader = AltOxiteDefaultPartialHeader;
                    conventions.PartialForEachOfBeforeEachItem = AltOxiteDefaultPartialBeforeEachItem;
                });

                // Default Behaviors for all actions -- ordered as they're executed
                /////////////////////////////////////////////////
                x.ByDefault.EveryControllerAction(d => d
                    .Will<load_the_current_principal>()

                    .Will<execute_the_result>()
                    .Will<set_the_current_site_details_on_the_output_viewmodel>()
                    );
                
                // Automatic controller registration
                /////////////////////////////////////////////////
                x.AddControllersFromAssembly.ContainingType<ViewModel>(c =>
                {
                    // All objects in Web.Controllers whose name ends with "*Controller"
                    // All public OMIOMO methods are actions, so no need to filter the methods
                    c.Where(t => 
                        t.Namespace.EndsWith("Web.Controllers") 
                        && t.Name.EndsWith("Controller"));

                    c.MapActionsWhere((m,i,o) => true);
                });

                // Manual overrides
                /////////////////////////////////////////////////
                x.OverrideConfigFor(DebugIndexAction, config =>
                {
                    config.PrimaryUrl = "__debug";
                });
            };

            Bootstrapper.Bootstrap();
        }

        private static HtmlExpressionBase AltOxiteDefaultPartialHeader(object itemList, int totalCount)
        {
            return new GenericOpenTagExpression("ul");
        }
        private static HtmlExpressionBase AltOxiteDefaultPartialBeforeEachItem(object item, int index, int total)
        {
            return new GenericOpenTagExpression("li");
        }

        private readonly Expression<Func<DebugController, object>> DebugIndexAction = c => c.Index(null);
    }
}
