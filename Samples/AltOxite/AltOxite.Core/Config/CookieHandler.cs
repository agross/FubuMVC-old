using System;
using System.Web;

namespace AltOxite.Core.Config
{
    public class CookieHandler : ICookieHandler
    {
        private readonly string _cookiePath;
        private string _userId;
        private bool _cookieIsThere;

        public CookieHandler(string cookie_path)
        {
            _cookiePath = cookie_path;
            _userId = "";
            _cookieIsThere = false;
        }

        public ICookieHandler ForHttpRequest(HttpRequest httpRequest)
        {
            if (httpRequest != null &&
                httpRequest.Cookies != null &&
                httpRequest.Cookies[_cookiePath] != null)
            {
                // ReSharper disable PossibleNullReferenceException
                _userId = HttpContext.Current.Request.Cookies[_cookiePath].Value;
                HttpContext.Current.Request.Cookies[_cookiePath].Expires = DateTime.Now.AddYears(1);
                // ReSharper restore PossibleNullReferenceException
                _cookieIsThere = !string.IsNullOrEmpty(_userId);
            }
            return this;
        }

        public string UserId
        {
            get { return _userId; }
        }

        public bool IsCookieThere
        {
            get { return _cookieIsThere; }
        }
    }
}