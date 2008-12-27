using System;
using AltOxite.Core.Web.Controllers;
using Microsoft.Practices.ServiceLocation;
using FubuMVC.Core.Controller.Config;

namespace AltOxite.Core.Web
{
    public static class UrlToExtensions
    {
        public static UrlToExpression UrlTo(this IAltOxitePage viewPage)
        {
            var resolver = ServiceLocator.Current.GetInstance<IUrlResolver>();

            return new UrlToExpression(resolver);
        }
    }

    public class UrlToExpression
    {
        private readonly IUrlResolver _resolver;

        public UrlToExpression(IUrlResolver resolver)
        {
            _resolver = resolver;
        }

        public string Home
        {
            get
            {
                return _resolver.UrlFor<HomeController>();
            }
        }

        public string Login
        {
            get
            {
                return _resolver.UrlFor<LoginController>();
            }
        }

        public string Logout
        {
            get
            {
                return _resolver.UrlFor<LoginController>(c => c.Logout(null));
            }
        }

    }
}
