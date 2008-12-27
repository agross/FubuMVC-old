using NUnit.Framework;
using FubuMVC.Core.Html;
using FubuMVC.Core.Html.Expressions;

namespace FubuMVC.Tests.Html.Expressions
{
    [TestFixture]
    public class ImageExpressionTester
    {
        [SetUp]
        public void SetUp() { UrlContext.Stub(""); }

        [Test]
        public void Extends_IFubuView_With_Image()
        {
            new ImageExpression("image.jpg")
                .ToString()
                .ShouldEqual("<img src=\"/content/images/image.jpg\"/>");
        }

        [Test]
        public void Extends_IFubuView_With_Image_With_Provided_Alt()
        {
            new ImageExpression("image.jpg")
                .Alt("test")
                .ToString()
                .ShouldEqual("<img src=\"/content/images/image.jpg\" alt=\"test\"/>");
        }

        [Test]
        public void Extends_IFubuView_With_Image_With_Different_Base_Path()
        {
            new ImageExpression("image.jpg")
                .BasedAt("~/test/")
                .ToString()
                .ShouldEqual("<img src=\"/test/image.jpg\"/>");
        }

        [Test]
        public void Extends_IFubuView_With_Image_With_Css_Class()
        {
            new ImageExpression("image.jpg")
                .Class("test")
                .ToString()
                .ShouldEqual("<img src=\"/content/images/image.jpg\" class=\"test\"/>");
        }

        [Test]
        public void Extends_IFubuView_With_Image_With_Id()
        {
            new ImageExpression("image.jpg")
                .ElementId("test")
                .ToString()
                .ShouldEqual("<img src=\"/content/images/image.jpg\" id=\"test\"/>");
        }
    }
}
