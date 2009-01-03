using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using AltOxite.Core.Domain;
using AltOxite.Core.Web;
using AltOxite.Core.Web.Behaviors;
using AltOxite.Core.Web.Controllers;
using AltOxite.Core.Web.DisplayModels;
using FubuMVC.Container.StructureMap.Config;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Html.Expressions;

namespace AltOxite.Web
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            ControllerConfig.Configure = x =>
            {
                x.UsingConventions( conventions =>
                {
                    conventions.PartialForEachOfHeader = AltOxiteDefaultPartialHeader;
                    conventions.PartialForEachOfBeforeEachItem = AltOxiteDefaultPartialBeforeEachItem;
                });

                // Default Behaviors for all actions
                /////////////////////////////////////////////////
                x.ByDefault.EveryControllerAction(d => d
                    .Will<set_the_current_site_details_on_the_output_viewmodel>()
                    .Will<set_the_current_logged_in_user_on_the_output_viewmodel>()
                    .Will<load_the_current_principal>()
                    .Will<set_up_default_data_the_first_time_this_app_is_run>()
                    .Will<execute_the_result>()
                    .Will<access_the_database_through_a_unit_of_work>());

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

                //-- Make the primary URL for logout be "/logout" instead of "login/logout"
                x.OverrideConfigFor(LogoutAction, config =>
                {
                    config.PrimaryUrl = "logout/";
                    config.RemoveAllBehaviors();
                    config.AddBehavior<execute_the_result>();
                });

                x.OverrideConfigFor(BlogPostIndexAction, config=>
                {
                    config.PrimaryUrl = "blog/{PostYear}/{PostMonth}/{PostDay}/{Slug}";
                });

                x.OverrideConfigFor(TagIndexAction, config=>
                {
                    config.PrimaryUrl = "tag/{Tag}";
                });
            };

            Bootstrapper.Bootstrap();
        }

        private static HtmlExpressionBase AltOxiteDefaultPartialHeader(object itemList, int totalCount)
        {
            var expr = new GenericOpenTagExpression("ul");
            
            if (itemList is IEnumerable<Tag>) expr.Class("tags");
            if (itemList is IEnumerable<CommentDisplay>) expr.Class("commented");

            return expr;
        }

        private static HtmlExpressionBase AltOxiteDefaultPartialBeforeEachItem(object item, int index, int total)
        {
            var expr = new GenericOpenTagExpression("li");

            if (index == 0) expr.Class("first");
            if (index >= (total - 1)) expr.Class("last");

            if (item is Comment && index % 2 != 0) expr.Class("odd");
            // TODO: Implement: sbClass.Append(comment.CreatorUser.IsAnonymous ? "anon " : comment.CreatorUser.ID == comment.Post.CreatorUser.ID ? "author " : "user ");

            return expr;
        }

        private readonly Expression<Func<LoginController, object>> LogoutAction = c => c.Logout(null);
        private readonly Expression<Func<BlogPostController, object>> BlogPostIndexAction = c => c.Index(null);
        private readonly Expression<Func<TagController, object>> TagIndexAction = c => c.Index(null);
    }
}
