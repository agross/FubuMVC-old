using AltOxite.Core.Domain;

namespace AltOxite.Core.Web
{
    public class ViewModel
    {
        public SiteConfiguration SiteConfig{ get; set; }
        public User CurrentUser { get; set; }
    }
}