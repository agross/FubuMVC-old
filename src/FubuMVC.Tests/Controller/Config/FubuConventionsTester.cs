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
            new FubuConventions().CanonicalControllerName(typeof (TestController)).ShouldEqual("test");
        }

        [Test]
        public void GetCanonicalControllerName_should_return_the_controller_type_name_lowered_and_stripped_of_controller_suffix()
        {
            new FubuConventions().GetCanonicalControllerName<TestController>().ShouldEqual("test");
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
            new FubuConventions().DefaultUrlForController(typeof (TestController)).ShouldEqual("test");
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

            var conv = new FubuConventions {ViewFileBasePath = "foo"};
            conv.DefaultPathToViewForAction(config).ShouldEqual("foo/test/someaction.aspx");
        }
    }
}