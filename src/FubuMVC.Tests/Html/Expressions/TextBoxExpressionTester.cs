using NUnit.Framework;
using FubuMVC.Core.Html.Expressions;
using FubuMVC.Core.View;

namespace FubuMVC.Tests.Html.Expressions
{
    [TestFixture]
    public class TextBoxExpressionTester
    {
        public class TestSite
        {
            public string Identifier { get; set; }
            public string Name { get; set; }
            public string Notes { get; set; }
        }

        public class TextBoxViewModel
        {
            public TestSite Site { get; set; }
            public string Value { get; set; }
        }

        [Test]
        public void should_use_prefix_for_name()
        {
            var viewModel = new TextBoxViewModel();

            new TextBoxExpression<TextBoxViewModel>(viewModel, c => c.Site.Notes, "prefix")
                .ToString().ShouldEqual("<input type=\"text\" name=\"prefixSiteNotes\" value=\"\" />");
        }

        [Test]
        public void should_add_specified_custom_html_attributes()
        {
            var viewModel = new TextBoxViewModel {Value = "Star Wars"};

            var expr = new TextBoxExpression<TextBoxViewModel>(viewModel, c => c.Value, "");
            expr.HtmlAttributes.Add("disabled", "disabled");

            expr.ToString().ShouldEqual(
                "<input type=\"text\" name=\"Value\" value=\"Star Wars\" disabled=\"disabled\" />");
        }

        [Test]
        public void should_render_a_textarea_when_multiline_option_is_enabled()
        {
            var viewModel = new TextBoxViewModel {Value = "Star Wars"};

            new TextBoxExpression<TextBoxViewModel>(viewModel, c => c.Value, "").MultilineMode()
                .ToString().ShouldEqual("<textarea name=\"Value\" >Star Wars</textarea>");
        }

        [Test]
        public void should_render_the_type_as_password_when_password_option_is_enabled()
        {
            var viewModel = new TextBoxViewModel {Value = "Star Wars"};

            new TextBoxExpression<TextBoxViewModel>(viewModel, c => c.Value, "").PasswordMode()
                .ToString().ShouldEqual("<input type=\"password\" name=\"Value\" value=\"Star Wars\" />");
        }

        [Test]
        public void should_use_the_correct_name_based_on_the_property_name_for_deep_properties()
        {
            var viewModel = new TextBoxViewModel();

            new TextBoxExpression<TextBoxViewModel>(viewModel, c => c.Site.Identifier, "")
                .ToString().ShouldEqual("<input type=\"text\" name=\"SiteIdentifier\" value=\"\" />");
        }

        [Test]
        public void should_use_the_correct_name_based_on_the_property_name_for_shallow_properties()
        {
            var viewModel = new TextBoxViewModel();

            new TextBoxExpression<TextBoxViewModel>(viewModel, c => c.Site, "")
                .ToString().ShouldEqual("<input type=\"text\" name=\"Site\" value=\"\" />");
        }

        [Test]
        public void should_use_the_correct_value_based_on_the_property_value()
        {
            var viewModel = new TextBoxViewModel {Value = "Star Wars"};

            new TextBoxExpression<TextBoxViewModel>(viewModel, c => c.Value, "")
                .ToString().ShouldEqual("<input type=\"text\" name=\"Value\" value=\"Star Wars\" />");
        }
    }
}