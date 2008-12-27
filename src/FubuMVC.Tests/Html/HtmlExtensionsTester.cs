using NUnit.Framework;
using FubuMVC.Core.Html;
using FubuMVC.Core.Html.Expressions;

namespace FubuMVC.Tests.Html
{
    [TestFixture]
    public class HtmlExtensionsTester
    {
        [Test]
        public void Extends_IFubuView_With_PageTile_Output_Html_Title_Tag_With_Provided_Name()
        {
            new TestView().PageTitle("test title").ShouldEqual("<title>test title</title>");
        }

        [Test]
        public void Extends_IFubuView_With_LinkTag()
        {
            new TestView().LinkTag().ShouldBeOfType(typeof(LinkExpression));
        }

        [Test]
        public void Extends_IFubuView_With_MetaTag()
        {
            new TestView().MetaTag().ShouldBeOfType(typeof(MetaExpression));
        }

        [Test]
        public void Extends_IFubuView_With_Script()
        {
            new TestView().Script("").ShouldBeOfType(typeof(ScriptReferenceExpression));
        }

        [Test]
        public void Extends_IFubuView_With_ScriptLink_Tag_With_Provided_Href()
        {
            new TestView().Script("Script.Url").ToString().ShouldEqual("<script type=\"text/javascript\" src=\"/content/scripts/Script.Url\"></script>");
        }

        [Test]
        public void Extends_IFubuView_With_ScriptLink_Tag_With_Provided_Href_As_IEnumerable()
        {
            var links = new[] { "Script.Url.1", "Script.Url.2" };
            new TestView().Script(links).ToString().ShouldEqual("<script type=\"text/javascript\" src=\"/content/scripts/Script.Url.1\"></script>\r\n<script type=\"text/javascript\" src=\"/content/scripts/Script.Url.2\"></script>");
        }

        [Test]
        public void Extends_IFubuView_With_ScriptLink_Tag_With_Provided_Href_As_IEnumerable_With_Indentation()
        {
            var links = new[] { "Script.Url.1", "Script.Url.2" };
            new TestView().Script(links).Indent("    ").ToString().ShouldEqual("<script type=\"text/javascript\" src=\"/content/scripts/Script.Url.1\"></script>\r\n    <script type=\"text/javascript\" src=\"/content/scripts/Script.Url.2\"></script>");
        }
    }
}
