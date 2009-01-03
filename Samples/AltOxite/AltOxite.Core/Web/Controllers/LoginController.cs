using System;
using AltOxite.Core.Services;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Controller;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Controller.Results;
using FubuMVC.Core.Security;

namespace AltOxite.Core.Web.Controllers
{
    public class LoginController
    {
        private readonly ISecurityDataService _securityDataService;
        private readonly IAuthenticationContext _authContext;
        private readonly IUrlResolver _resolver;

        public LoginController(ISecurityDataService securityDataService, IAuthenticationContext authContext, IUrlResolver resolver)
        {
            _securityDataService = securityDataService;
            _authContext = authContext;
            _resolver = resolver;
        }

        public LoginViewModel Index(LoginViewModel loginModel)
        {
            if ( loginModel.HasCredentials())
            {
                var userId = _securityDataService.AuthenticateForUserId(loginModel.Username, loginModel.Password);
                
                if (userId.HasValue)
                {
                    _authContext.ThisUserHasBeenAuthenticated(userId.Value.ToString(), loginModel.RememberMeChecked);

                    loginModel.ErrorMessage = null;
                    loginModel.AuthenticationSuccessful = true;

                    var redirectUrl = loginModel.ReturnUrl.IsNotEmpty()
                                          ? loginModel.ReturnUrl
                                          : _resolver.Home();

                    loginModel.ResultOverride = new RedirectResult(redirectUrl);

                    return loginModel;
                }

                loginModel.ErrorMessage = "Invalid username or password";
            }
            
            return loginModel;
        }

        public ViewModel Logout(ViewModel model)
        {
            _authContext.SignOut();
            var logoutModel = new LogoutViewModel
                {
                    ResultOverride = new RedirectResult(_resolver.Home())
                };
            ;
            return logoutModel;
        }
    }

    public class LogoutViewModel : ViewModel, ISupportResultOverride
    {
        public IInvocationResult ResultOverride { get; set; }
    }

    [Serializable]
    public class LoginViewModel : ViewModel, ISupportResultOverride
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberMeChecked { get; set; }
        public string ErrorMessage { get; set; }
        public string ReturnUrl { get; set; }
        public bool AuthenticationSuccessful { get; set; }
        
        public bool HasCredentials()
        {
            return Username.IsNotEmpty() && Password.IsNotEmpty();
        }

        public IInvocationResult ResultOverride { get; set; }
    }
}