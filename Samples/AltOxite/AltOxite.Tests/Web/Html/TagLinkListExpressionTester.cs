using System.Collections.Generic;
using AltOxite.Core.Domain;
using AltOxite.Core.Web.Html;
using FubuMVC.Core.Html.Expressions;
using FubuMVC.Core.View.WebForms;
using NUnit.Framework;
using Rhino.Mocks;

namespace AltOxite.Tests.Web.Html
{
    [TestFixture]
    public class TagLinkListExpressionTester
    {
        private IWebFormsViewRenderer _renderer;
        private TagLinkListExpression _expression;
        private IEnumerable<Tag> _tags;

        [SetUp]
        public void SetUp()
        {
            _renderer = MockRepository.GenerateStub<IWebFormsViewRenderer>();
            _expression = new TagLinkListExpression(null, _renderer);
            _tags = new List<Tag>
            {
                new Tag(),
                new Tag()
            };
        }
        [Test]
        public void should_be_of_type_tagcollection()
        {
            _expression
                .ForEach(_tags)
                .Display<TagLink>()
                .RenderExpression
                .ShouldBeOfType<RenderPartialExpression.RenderPartialForScope<TagLink>>();
        }
    }
}