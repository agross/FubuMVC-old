using NUnit.Framework;
using FubuMVC.Core.Html;
using FubuMVC.Core.Html.Expressions;

namespace FubuMVC.Tests.Html.Expressions
{
    [TestFixture]
    public class LinkExpressionTester
    {
        [SetUp]
        public void SetUp() { UrlContext.Stub(""); }

        [Test]
        public void Extends_IFubuView_With_Link_Tag_With_Provided_Relationship_Type_Href_Title()
        {
            new LinkExpression()
                .Rel("EditURI")
                .Href("RsdLink")
                .Type("application/rsd+xml")
                .Title("RSD")
                .ToString()
                .ShouldEqual("<link rel=\"EditURI\" type=\"application/rsd+xml\" title=\"RSD\" href=\"/RsdLink\"/>");
        }

        [Test]
        public void Extends_IFubuView_With_PingBackLink_Tag_With_Provided_Href()
        {
            new LinkExpression()
                .Href("/Pingback.Url")
                .AsPingBack()
                .ToString()
                .ShouldEqual("<link rel=\"pingback\" href=\"/Pingback.Url\"/>");
        }

        [Test]
        public void Extends_IFubuView_With_AlternateLink_Tag_With_Provided_Href_Type_Title()
        {
            new LinkExpression()
                .Href("Alternate.Url")
                .Type("application/atom+xml")
                .Title("Title Atom")
                .AsAlternate()
                .ToString()
                .ShouldEqual("<link type=\"application/atom+xml\" title=\"Title Atom\" rel=\"alternate\" href=\"/Alternate.Url\"/>");
        }
        [Test]
        public void Extends_IFubuView_With_AlternateLink_Tag_With_Provided_AlternateLink_As_IEnumerable()
        {
            var links = new[]
            {
                new {Href = "link1", Title = "Link one", Type = "application/atom+xml"},
                new {Href = "link2", Title = "Link two", Type = "application/rss+xml"}
            };
            new LinkExpression()
                .FromList(links, (x,l)=> l.AsAlternate().Href(x.Href).Type(x.Type).Title(x.Title))
                //.AsAlternate()
                .ToString()
                .ShouldEqual("<link rel=\"alternate\" type=\"application/atom+xml\" title=\"Link one\" href=\"/link1\"/>\r\n<link rel=\"alternate\" type=\"application/rss+xml\" title=\"Link two\" href=\"/link2\"/>");
        }
        [Test]
        public void Extends_IFubuView_With_AlternateLink_Tag_With_Provided_AlternateLink_As_IEnumerable_With_Indentation()
        {
            var links = new[]
            {
                new {Href = "link1", Title = "Link one", Type = "application/atom+xml"},
                new {Href = "link2", Title = "Link two", Type = "application/rss+xml"}
            };
            new LinkExpression()
                .FromList(links, (x, l)=> l.AsAlternate().Href(x.Href).Type(x.Type).Title(x.Title))
                .Indent("    ")
                .ToString()
                .ShouldEqual("<link rel=\"alternate\" type=\"application/atom+xml\" title=\"Link one\" href=\"/link1\"/>\r\n    <link rel=\"alternate\" type=\"application/rss+xml\" title=\"Link two\" href=\"/link2\"/>");
        }

        [Test]
        public void Extends_IFubuView_With_OpenSearchLink_Tag_With_Provided_Href_Title()
        {
            new LinkExpression()
                .Href("OpenSearch.Url")
                .Title("OpenSearch.Title")
                .AsOpenSearch()
                .ToString()
                .ShouldEqual("<link title=\"OpenSearch.Title\" rel=\"search\" type=\"application/opensearchdescription+xml\" href=\"/OpenSearch.Url\"/>");
        }

        [Test]
        public void Extends_IFubuView_AsStylesheet()
        {
            new LinkExpression()
                .Href("x")
                .AsStyleSheet()
                .ToString()
                .ShouldEqual(@"<link rel=""stylesheet"" type=""text/css"" href=""/x""/>");
        }

        [Test]
        public void Extends_IFubuView_AsStylesheet_With_Media_Defined()
        {
            new LinkExpression()
                .Href("x")
                .Media("all")
                .AsStyleSheet()
                .ToString()
                .ShouldEqual(@"<link media=""all"" rel=""stylesheet"" type=""text/css"" href=""/x""/>");
        }
    }
}
