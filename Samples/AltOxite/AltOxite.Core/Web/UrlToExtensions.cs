using Microsoft.Practices.ServiceLocation;
using FubuMVC.Core.Controller.Config;

namespace AltOxite.Core.Web
{
    public static class UrlToExtensions
    {
        public static IUrlResolver UrlTo(this IAltOxitePage viewPage)
        {
            return ServiceLocator.Current.GetInstance<IUrlResolver>();
        }
    }
}
