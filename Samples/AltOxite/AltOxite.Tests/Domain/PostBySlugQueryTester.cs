using System.Globalization;
using System.Threading;
using AltOxite.Core.Domain;
using NUnit.Framework;

namespace AltOxite.Tests.Domain
{
    [TestFixture]
    public class PostBySlugQueryTester
    {
        private CultureInfo _originalCulture;
        private string _capitalARing;
        private string _aRing;
        private string _slug;
        private Post _post;

        [SetUp]
        public void SetUp()
        {
            _originalCulture = Thread.CurrentThread.CurrentCulture;

            // Define a target string to search for.
            // U+00c5 = LATIN CAPITAL LETTER A WITH RING ABOVE
            _capitalARing = "\u00c5";

            // Define a string to search. 
            // The result of combining the characters LATIN SMALL LETTER A and COMBINING 
            // RING ABOVE (U+0061, U+030a) is linguistically equivalent to the character 
            // LATIN SMALL LETTER A WITH RING ABOVE (U+00e5).
            _aRing = "\u0061\u030a";

            _slug = "TESTSLUG";
            _post = new Post { Slug= _slug.ToLower() };
        }

        [TearDown]
        public void TearDown()
        {
            Thread.CurrentThread.CurrentCulture = _originalCulture;
        }

        [Test]
        public void should_be_case_insensitive()
        {
            new PostBySlug(_slug).Expression.Compile()(_post).ShouldBeTrue();
        }

        [Test]
        public void should_be_culture_agnostic_en_US()
        {
            var query = new PostBySlug(_capitalARing);
            _post.Slug = _aRing;
            
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

            query.Expression.Compile()(_post).ShouldBeTrue();
        }

        [Test]
        public void should_be_culture_agnostic_sv_SE()
        {
            var query = new PostBySlug(_capitalARing);
            _post.Slug = _aRing;

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("sv-SE");

            query.Expression.Compile()(_post).ShouldBeTrue();
        }
    }
}