using NUnit.Framework;
using FubuMVC.Core.Web.Security;

namespace FubuMVC.Tests.Web.Security
{
    [TestFixture]
    public class WebAuthenticationContextTester
    {
        private WebAuthenticationContext _authSvc;
        private string _actualUser;
        private bool? _actualRememberMe;
        private bool _signOutCalled;

        [SetUp]
        public void SetUp()
        {
            _actualUser = null;
            _actualRememberMe = null;
            _signOutCalled = false;

            _authSvc = new WebAuthenticationContext
                {
                    SetAuthCookieFunc = ((u, r) =>
                    {
                        _actualUser = u;
                        _actualRememberMe = r;
                    }),

                    SignOutFunc = () => { _signOutCalled = true; }
                };
        }

        [Test]
        public void should_set_the_forms_auth_token_with_the_rememberMe_setting()
        {
            var username = "user";
            
            _authSvc.ThisUserHasBeenAuthenticated(username, true);

            _actualUser.ShouldEqual(username);
            _actualRememberMe.Value.ShouldBeTrue();
        }

        [Test]
        public void SignOut_should_signout_from_forms_auth()
        {
            _authSvc.SignOut();

            _signOutCalled.ShouldBeTrue();
        }
    }
}