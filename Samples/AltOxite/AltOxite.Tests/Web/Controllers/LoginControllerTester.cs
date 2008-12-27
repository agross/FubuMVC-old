using System;
using AltOxite.Core.Services;
using AltOxite.Core.Web.Controllers;
using NUnit.Framework;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Security;
using Rhino.Mocks;

namespace AltOxite.Tests.Web.Controllers
{
    [TestFixture]
    public class LoginController_Index
    {
        [Test]
        public void index_should_do_nothing_but_just_render_the_view_if_nothing_was_supplied()
        {
            new LoginController(null, null, null).Index(new LoginViewModel()).ShouldBeOfType<LoginViewModel>();
        }
    }

    [TestFixture]
    public class LoginController_invalid_login
    {
        private LoginController _controller;
        private ISecurityDataService _security;
        private IAuthenticationContext _authContext;
        private LoginViewModel _loginModel;
        private IUrlResolver _resolver;

        [SetUp]
        public void SetUp()
        {
            _security = MockRepository.GenerateStub<ISecurityDataService>();
            _authContext = MockRepository.GenerateMock<IAuthenticationContext>();
            _resolver = MockRepository.GenerateMock<IUrlResolver>();
            _controller = new LoginController(_security, _authContext, _resolver);

            _loginModel = new LoginViewModel();
            _security.Stub(s => s.AuthenticateForUserId("foo", "bar")).Return(null);
        }
        
        [Test, Ignore("Will have to implement this later")]
        public void login_should_return_an_error_if_no_password_is_specified()
        {
            _loginModel.Username = "foo";

            _controller.Index(_loginModel).ErrorMessage.ShouldNotBeNull();
        }

        [Test]
        public void login_should_return_an_error_if_the_username_and_password_are_incorrect()
        {
            _loginModel.Username = "foo";
            _loginModel.Password = "bar";

            _controller.Index(_loginModel);

            _loginModel.ErrorMessage.ShouldNotBeNull();
        }
    }

    [TestFixture]
    public class LoginController_valid_login
    {
        private LoginController _controller;
        private ISecurityDataService _security;
        private IAuthenticationContext _authContext;
        private LoginViewModel _loginModel;
        private LoginViewModel _result;
        private string _returnUrl;
        private IUrlResolver _resolver;
        private Guid _userId;

        [SetUp]
        public void SetUp()
        {
            _security = MockRepository.GenerateStub<ISecurityDataService>();
            _authContext = MockRepository.GenerateMock<IAuthenticationContext>();
            _resolver = MockRepository.GenerateMock<IUrlResolver>();
            _controller = new LoginController(_security, _authContext, _resolver);

            _userId = Guid.NewGuid();
            _security.Stub(s => s.AuthenticateForUserId("foo", "bar")).Return(_userId);

            _returnUrl = "TESTRETURN";
            _loginModel = new LoginViewModel {Username = "foo", Password = "bar", RememberMeChecked = true, ReturnUrl=_returnUrl};
            
            _result = _controller.Index(_loginModel);
        }

        [Test]
        public void should_not_return_an_error_if_the_username_and_password_are_valid()
        {
            _result.ErrorMessage.ShouldBeNull();
        }

        [Test]
        public void should_setup_the_user_as_authenticated()
        {
            _authContext.AssertWasCalled(a => a.ThisUserHasBeenAuthenticated(_userId.ToString(), _loginModel.RememberMeChecked));
        }

        [Test]
        public void should_set_the_authentication_successful_switch_on_the_viewmodel()
        {
            _result.AuthenticationSuccessful.ShouldBeTrue();
        }
    }
}