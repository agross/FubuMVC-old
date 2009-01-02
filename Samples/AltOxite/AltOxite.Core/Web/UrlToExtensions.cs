using AltOxite.Core.Domain;
using AltOxite.Core.Web.Controllers;
using AltOxite.Core.Web.DisplayModels;
using Microsoft.Practices.ServiceLocation;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core;

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

        public string Tag(string tagName)
        {
            return ("~/Tag/" + tagName).ToFullUrl(); // TODO: _resolver.UrlFor<TagController>() + "/" + tagName;
        }

        public string PublishedPost(PostDisplay post)
        {
            return ("~/Blog/" + 
                post.Published.Year + "/" + 
                post.Published.Month + "/" + 
                post.Published.Day + "/" + 
                post.Slug).ToFullUrl();
        }

    }
}
