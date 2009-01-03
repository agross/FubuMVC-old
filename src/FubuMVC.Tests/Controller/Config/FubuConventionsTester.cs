using FubuMVC.Core.Controller.Config;
using NUnit.Framework;

namespace FubuMVC.Tests.Controller.Config
{
    [TestFixture]
    public class FubuConventionsTester
    {
        [Test]
        public void CanonicalControllerName_should_return_the_controller_type_name_lowered_and_stripped_of_controller_suffix()
        {
            new FubuConventions().CanonicalControllerName(typeof(TestController)).ShouldEqual("test");
        }

        [Test]
        public void GetCanonicalControllerName_should_return_the_controller_type_name_lowered_and_stripped_of_controller_suffix()
        {
            new FubuConventions().GetCanonicalControllerName<TestController>().ShouldEqual("test");
        }

        [Test]
        public void UrlRouteParametersForAction_should_return_url_formated_route_parameters_for_UrlRequired_input_viewmodel_properties()
        {
            var config = ControllerActionConfig.ForAction<TestController, TestInputRequiredModel, TestOutputModel>(
                (c, i) => c.RequiredParamsAction(i));

            new FubuConventions().UrlRouteParametersForAction(config).ShouldStartWith("/{Prop1}");
        }

        [Test]
        public void UrlRouteParametersForAction_should_preserve_url_parameter_ordering_as_declared_on_the_input_type()
        {
            var config = ControllerActionConfig.ForAction<TestController, TestInputRequiredModel, TestOutputModel>(
                (c, i) => c.RequiredParamsAction(i));

            new FubuConventions().UrlRouteParametersForAction(config).ShouldEqual("/{Prop1}/{Prop3}/{Prop2}");
        }

        [Test]
        public void PrimaryUrlConvention_should_default_to_the_controller_canonical_name_and_action_name()
        {
            var config = ControllerActionConfig.ForAction<TestController, TestInputModel, TestOutputModel>(
                (c, i) => c.SomeAction(i));

            new FubuConventions().PrimaryUrlConvention(config).ShouldEqual("test/someaction");
        }

        [Test]
        public void SetDefaults_should_reset_user_defined_conventions()
        {
            var conv = new FubuConventions();
            var orig = conv.CanonicalControllerName;

            conv.CanonicalControllerName = null;

            conv.SetDefaults();

            conv.CanonicalControllerName.ShouldEqual(orig);
        }

        [Test]
        public void DefaultPathForController_should_default_to_controller_canonical_name()
        {
            new FubuConventions().DefaultUrlForController(typeof(TestController)).ShouldEqual("test");
        }

        [Test]
        public void IsAppDefaultUrl_should_default_to_looking_for_Home_controller_Index_action()
        {
            var convention = new FubuConventions
                {
                    PrimaryUrlConvention = (config => "home/index")
                };

            convention.IsAppDefaultUrl(null).ShouldBeTrue();
        }

        [Test]
        public void ViewFileBasePath_default_should_be_the_Views_app_relative_folder()
        {
            new FubuConventions().ViewFileBasePath.ShouldEqual("~/Views");
        }

        [Test]
        public void DefaultPathToViewForAction_should_be_view_base_path_plus_controller_canon_name_plus_action_name()
        {
            var config = ControllerActionConfig.ForAction<TestController, TestInputModel, TestOutputModel>(
                (c, i) => c.SomeAction(i));

            var conv = new FubuConventions { ViewFileBasePath = "foo"};
            conv.DefaultPathToViewForAction(config).ShouldEqual("foo/test/someaction.aspx");
        }

        [Test]
        public void DefaultPathToPartial_should_be_shared_view_base_path_plus_shared_folder_then_action_name()
        {
            var conv = new FubuConventions { SharedViewFileBasePath = "foo" };

            conv.DefaultPathToPartialView(typeof(TestPartialView)).ShouldEqual("foo/TestPartialView.ascx");
        }

        [Test]
        public void PartialForEachOfHeader_should_default_to_a_ul_open_tag()
        {
            var conv = new FubuConventions();

            conv.PartialForEachOfHeader(null, 0).ToString().ShouldEqual("<ul>");
        }

        [Test]
        public void PartialForEachOfBeforeEachItem_should_render_li_open_tag()
        {
            var conv = new FubuConventions();

            conv.PartialForEachOfBeforeEachItem(null, 0, 0).ToString().ShouldEqual("<li>");
        }

        [Test]
        public void PartialForEachOfAfterEachItem_should_render_li_close_tag()
        {
            var conv = new FubuConventions();

            conv.PartialForEachOfAfterEachItem(null, 0, 0).ShouldEqual("</li>");
        }

        [Test]
        public void PartialForEachOfFooter_should_render_ul_close_tag()
        {
            var conv = new FubuConventions();

            conv.PartialForEachOfFooter(null, 0).ShouldEqual("</ul>");
        }

        private class TestPartialView

        { }
    }
}