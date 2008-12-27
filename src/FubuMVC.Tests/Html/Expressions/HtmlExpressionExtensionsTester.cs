using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using FubuMVC.Core.Html.Expressions;

namespace FubuMVC.Tests.Html.Expressions
{
    [TestFixture]
    public class HtmlExpressionExtensionsTester
    {
        [Test]
        public void Attr_adds_an_attribute_and_value_to_the_expression()
        {
            new TestExpression().Attr("foo", "bar").HtmlAttributes["foo"].ShouldEqual("bar");
        }

        [Test]
        public void ReadOnly_sets_the_disabled_attribute()
        {
            new TestExpression().ReadOnly().HtmlAttributes["disabled"].ShouldEqual("disabled");
        }

        [Test]
        public void Required_adds_the_x_required_CSS_class()
        {
            new TestExpression().Required().CssClasses.ShouldContain("x-required");
        }

        [Test]
        public void Attributes_sets_HTML_attributes_based_on_properties_from_an_anonymous_types()
        {
            new TestExpression().Attributes(new { foo = "bar", baz = "bang" }).HtmlAttributes.ToList().ShouldHaveTheSameElementsAs(
                (IList)new Dictionary<string, string> { { "foo", "bar" }, { "baz", "bang" } }.ToList());
        }

        [Test]
        public void Class_sets_the_CSS_class()
        {
            new TestExpression().Class("foo").Class("bar").CssClasses.ShouldHaveTheSameElementsAs(new[] { "foo", "bar" });
        }

        [Test]
        public void Id_sets_the_id_attribute()
        {
            new TestExpression().ElementId("foo").HtmlAttributes["id"].ShouldEqual("foo");
        }

        [Test]
        public void Width_sets_the_width_style()
        {
            new TestExpression().Width(99).CustomStyles["width"].ShouldEqual("99px");
        }

        [Test]
        public void WidthPercent_sets_the_width_style()
        {
            new TestExpression().WidthPercent(99).CustomStyles["width"].ShouldEqual("99%");
        }

        [Test]
        public void Height_sets_the_height_style()
        {
            new TestExpression().Height(99).CustomStyles["height"].ShouldEqual("99px");
        }

        [Test]
        public void Style_adds_a_new_style_key_value()
        {
            new TestExpression().Style("foo", "bar").CustomStyles["foo"].ShouldEqual("bar");
        }

        [Test]
        public void VisibilityFrom_sets_the_display_style()
        {
            new TestExpression().VisibilityFrom(true).CustomStyles.ContainsKey("display").ShouldBeFalse();
            new TestExpression().VisibilityFrom(false).CustomStyles["display"].ShouldEqual("none");
        }


        public class TestExpression : HtmlExpressionBase
        {
        }
    }
}