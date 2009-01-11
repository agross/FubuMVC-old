using AltOxite.Core.Web.Controllers;
using AltOxite.Core.Web.DisplayModels;
using FubuMVC.Core;
using FubuMVC.Core.Controller.Config;

namespace AltOxite.Core.Web
{
    public static class Well_known_URLs
    {
        public static string Home(this IUrlResolver resolver)
        {
            return resolver.UrlFor<HomeController>();
        }

        public static string Login(this IUrlResolver resolver)
        {
            return resolver.UrlFor<LoginController>();
        }

        public static string Logout(this IUrlResolver resolver)
        {
            return resolver.UrlFor<LoginController>(c => c.Logout(null));
        }

        public static string CommentToPublishedPost(this IUrlResolver resolver, PostDisplay post)
        {
            // TODO: _resolver.UrlFor<BlogController>() + "/" + ...;
            return ("~/blog/" +
                    post.Published.Year + "/" +
                    post.Published.Month + "/" +
                    post.Published.Day + "/" +
                    post.Slug + "/comment").ToFullUrl();
        }

        public static string PublishedPost(this IUrlResolver resolver, PostDisplay post)
        {
            // TODO: _resolver.UrlFor<BlogController>() + "/" + ...;
            return ("~/blog/" +
                    post.Published.Year + "/" +
                    post.Published.Month + "/" +
                    post.Published.Day + "/" +
                    post.Slug).ToFullUrl();
        }

        public static string Tag(this IUrlResolver resolver, string tagName)
        {
            return ("~/Tag/" + tagName).ToFullUrl(); // TODO: _resolver.UrlFor<TagController>() + "/" + tagName;
        }

        public static string PageNotFound(this IUrlResolver resolver)
        {
            return ("~/404").ToFullUrl(); // TODO: _resolver.UrlFor<PageNotFoundController>();
        }
    }
}