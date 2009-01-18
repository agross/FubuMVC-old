using Microsoft.Practices.ServiceLocation;
using FubuMVC.Core.Controller.Config;

namespace VsTemplate.Core.Web
{
    public static class UrlToExtensions
    {
        public static IUrlResolver UrlTo(this IFubuMvcPage viewPage)
        {
            return ServiceLocator.Current.GetInstance<IUrlResolver>();
        }
    }
}
