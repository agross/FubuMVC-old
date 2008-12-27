using NUnit.Framework;
using FubuMVC.Core.Html.Expressions;

namespace FubuMVC.Tests.Html.Expressions
{
    [TestFixture]
    public class MetaExpressionTester
    {
        [Test]
        public void Extends_IFubuView_With_Meta_Tag_With_Provided_Name_And_Content()
        {
            new MetaExpression()
                .Name("language")
                .Content("english")
                .ToString()
                .ShouldEqual("<meta name=\"language\" content=\"english\"/>");
        }

        [Test]
        public void Extends_IFubuView_With_Meta_Tag_With_Provided_Name_And_Content_As_IEnumerable()
        {
            var keywords = new[]
            {
                "keyword1",
                "keyword2"
            };

            new MetaExpression()
                .Name("keywords")
                .Content(keywords)
                .ToString()
                .ShouldEqual("<meta name=\"keywords\" content=\"keyword1, keyword2\"/>");
        }

        [Test]
        public void Extends_IFubuView_With_Meta_Tag_With_Provided_Name_And_Empty_Content_As_IEnumerable()
        {
            new MetaExpression()
                .Name("keywords")
                .Content(new string[]{})
                .ToString()
                .ShouldEqual("<meta name=\"keywords\" content=\"\"/>");
        }

        [Test]
        public void Extends_IFubuView_With_Meta_As_Content_Type()
        {
            new MetaExpression()
                .AsContentType()
                .ToString()
                .ShouldEqual("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"/>");
        }
    }
}
