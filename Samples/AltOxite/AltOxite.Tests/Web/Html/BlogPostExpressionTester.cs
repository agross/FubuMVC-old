using System.Collections.Generic;
using AltOxite.Core.Domain;
using AltOxite.Core.Web.Controllers;
using AltOxite.Core.Web.Html;
using FubuMVC.Core.Html.Expressions;
using FubuMVC.Core.View.WebForms;
using NUnit.Framework;
using Rhino.Mocks;

namespace AltOxite.Tests.Web.Html
{
    [TestFixture]
    public class BlogPostExpressionTester
    {
        private IWebFormsViewRenderer _renderer;
        private BlogPostExpression _expression;
        private IEnumerable<Post> _posts;

        [SetUp]
        public void SetUp()
        {
            _renderer = MockRepository.GenerateStub<IWebFormsViewRenderer>();
            _expression = new BlogPostExpression(null, _renderer);
            _posts = new List<Post>
            {
                new Post(),
                new Post()
            };
        }

        [Test]
        public void should_be_of_type_blogpost()
        {
            _expression.ForEach(_posts)
                .Display<BlogPost>(null)
                .RenderExpression
                .ShouldBeOfType<RenderPartialExpression.RenderPartialForScope<BlogPost>>();
        }
    }
}