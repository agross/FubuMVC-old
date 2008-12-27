using AltOxite.Core.Domain;
using AltOxite.Core.Web;
using AltOxite.Core.Web.Html;
using AltOxite.Core.Web.WebForms;
using NUnit.Framework;
using FubuMVC.Core.Html.Expressions;
using FubuMVC.Core.View.WebForms;
using Rhino.Mocks;

namespace AltOxite.Tests.Web.Html
{
    [TestFixture]
    public class LoginStatusExpressionTester
    {
        private IWebFormsViewRenderer _renderer;
        private User _user;
        private LoginStatusExpression _expression;

        [SetUp]
        public void SetUp()
        {
            _renderer = MockRepository.GenerateStub<IWebFormsViewRenderer>();
            _user = new User();
            _expression = new LoginStatusExpression(null, _renderer);
        }

        [Test]
        public void should_use_the_logged_in_view_when_user_is_not_null()
        {
            _expression.For(_user)
                .WhenLoggedInShow<TestLoggedInViewControl>()
                .RenderExpression
                    .ShouldBeOfType<RenderPartialExpression.RenderPartialForScope<TestLoggedInViewControl>>();
        }

        [Test]
        public void should_use_the_logged_out_view_when_user_is_null()
        {
            _expression.For(null)
                .WhenLoggedOutShow<TestLoggedOutViewControl>()
                .RenderExpression
                    .ShouldBeOfType<RenderPartialExpression.RenderPartialForScope<TestLoggedOutViewControl>>();
        }

        public class TestLoggedInViewControl : AltOxiteUserControl<ViewModel>
        {
        }

        public class TestLoggedOutViewControl : AltOxiteUserControl<ViewModel>
        {
        }
    }
}