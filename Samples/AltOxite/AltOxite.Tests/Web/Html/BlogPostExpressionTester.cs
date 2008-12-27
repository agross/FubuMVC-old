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
        public void should_use_the_logged_in_view_when_user_is_not_null()
        {
            _expression.ForEach(_posts)
                .Display<BlogPost>()
                .RenderExpression
                .ShouldBeOfType<RenderPartialExpression.RenderPartialForScope<BlogPost>>();
        }
    }
}