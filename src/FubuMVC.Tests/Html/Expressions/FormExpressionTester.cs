using FubuMVC.Core.Html.Expressions;
using NUnit.Framework;

namespace FubuMVC.Tests.Html.Expressions
{
    [TestFixture]
    public class FormExpressionTester
    {
        private FormExpression _formExpression;

        [SetUp]
        public void SetUp()
        {
            _formExpression = new FormExpression("/SomeTestUrl");
        }

        [Test]
        public void Default_form_method_should_be_post()
        {
            _formExpression.ToString().ShouldContain("method=\"post\"");
        }

        [Test]
        public void AsGet_should_change_form_method_to_get()
        {
            _formExpression.AsGet();
            _formExpression.ToString().ShouldContain("method=\"get\"");
        }
    }
}