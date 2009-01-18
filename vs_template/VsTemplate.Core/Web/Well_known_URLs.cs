using FubuMVC.Core;
using VsTemplate.Core.Web.Controllers;
using FubuMVC.Core.Controller.Config;

namespace VsTemplate.Core.Web
{
    public static class Well_known_URLs
    {
        public static string Home(this IUrlResolver resolver)
        {
            return resolver.UrlFor<HomeController>();
        }
        public static string Debug(this IUrlResolver resolver)
        {
            return "~/__{0}".ToFormat("debug").ToFullUrl();
        }
    }
}