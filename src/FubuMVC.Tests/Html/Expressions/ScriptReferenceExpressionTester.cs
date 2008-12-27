using NUnit.Framework;
using FubuMVC.Core.Html;
using FubuMVC.Core.Html.Expressions;

namespace FubuMVC.Tests.Html.Expressions
{
    [TestFixture]
    public class ScriptReferenceExpressionTester
    {
        //[SetUp]
        //public void SetUp()
        //{
        //    UrlContext.Stub();
        //}

        [Test]
        public void should_default_base_url()
        {
            new ScriptReferenceExpression().Add("x").ToString().ShouldContain(@"""/content/scripts/x""");
        }

        [Test]
        public void should_use_relative_url_given_by_caller()
        {
            new ScriptReferenceExpression().Add("foo").ToString().ShouldContain("/content/scripts/foo");
        }

        [Test]
        public void should_use_base_url_given_by_caller()
        {
            new ScriptReferenceExpression().Add("foo").BasedAt("bar/").ToString().ShouldContain("/bar/foo");
        }

        [Test]
        public void should_do_nothing_if_url_given_is_empty()
        {
            new ScriptReferenceExpression().Add(null).ToString().ShouldBeEmpty();
        }
    }
}
