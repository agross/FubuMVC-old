using NUnit.Framework;
using FubuMVC.Core.Html.Expressions;

namespace FubuMVC.Tests.Html.Expressions
{
    [TestFixture]
    public class CheckboxExpressionTester
    {
        public string Prefix { get; set; }

        [SetUp]
        public void SetUp()
        {
            Prefix = string.Empty;
        }

        [Test]
        public void should_set_the_correct_checked_value_from_the_viewmodel()
        {
            var viewModel = new CheckBoxViewModel { Active = true, Value = true };

            new CheckboxExpression<CheckBoxViewModel>(viewModel, c => c.Active, Prefix)
                .ToString().ShouldEqual("<input type=\"checkbox\" value=\"Active\" name=\"Active\" checked=\"checked\"/>");
        }


        [Test]
        public void should_set_the_correct_name_with_prefix()
        {
            Prefix = "Primary";
            var viewModel = new CheckBoxViewModel { Active = true, Value = true };

            new CheckboxExpression<CheckBoxViewModel>(viewModel, c => c.Active, Prefix)
                .ToString().ShouldEqual("<input type=\"checkbox\" value=\"PrimaryActive\" name=\"PrimaryActive\" checked=\"checked\"/>");
        }

        [Test]
        public void should_set_the_correct_checked_value_from_the_viewmodel_for_false()
        {
            var viewModel = new CheckBoxViewModel { Active = false, Value = true };

            new CheckboxExpression<CheckBoxViewModel>(viewModel, c => c.Active, Prefix)
                .ToString().ShouldEqual("<input type=\"checkbox\" value=\"Active\" name=\"Active\"/>");
        }

        [Test]
        public void should_add_specified_custom_html_attributes()
        {
            var viewModel = new CheckBoxViewModel { Active = true, Value = true };

            var expr = new CheckboxExpression<CheckBoxViewModel>(viewModel, c => c.Active, Prefix);
            expr.HtmlAttributes.Add("disabled", "disabled");

            expr.ToString().ShouldEqual("<input type=\"checkbox\" value=\"Active\" name=\"Active\" checked=\"checked\" disabled=\"disabled\"/>");
        }


        public class CheckBoxViewModel
        {
            public bool Active { get; set; }
            public bool Value { get; set; }
        }
    }
}