using System;
using System.Web;
using AltOxite.Core.Config;
using AltOxite.Core.Domain;
using AltOxite.Core.Persistence;
using FubuMVC.Core.Behaviors;

namespace AltOxite.Core.Web.Behaviors
{
    public class set_user_from_http_cookie_if_current_user_is_not_authenticated : behavior_base_for_convenience
    {
        private readonly IRepository _repository;
        private readonly ICookieHandler _cookieHandler;

        public set_user_from_http_cookie_if_current_user_is_not_authenticated(IRepository repository, ICookieHandler cookieHandler)
        {
            _repository = repository;
            _cookieHandler = cookieHandler;
            // TODO somehow get access to the HttpRequest to read the cookie
            //if (HttpContext.Current != null)
            //    _cookieHandler.ForHttpRequest(HttpContext.Current.Request);
        }

        public void UpdateModel(ViewModel model)
        {
            if (model == null) return;

            if (model.CurrentUser.IsAuthenticated) return;

            if (!_cookieHandler.IsCookieThere) return;

            Guid UserId = new Guid(_cookieHandler.UserId);

            var user = _repository.Load<User>(UserId);
            if (user != null)
            {
                user.IsAuthenticated = false;
                model.CurrentUser = user;
            }
        }

        public override void ModifyOutput<OUTPUT>(OUTPUT output)
        {
            UpdateModel(output as ViewModel);
        }
    }
}