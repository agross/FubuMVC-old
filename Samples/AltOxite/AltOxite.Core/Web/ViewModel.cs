using AltOxite.Core.Domain;

namespace AltOxite.Core.Web
{
    public class ViewModel
    {
        public string SiteName { get; set; }
        public string LanguageDefault { get; set; }
        public string SEORobots { get; set; }
        public User CurrentUser { get; set; }
    }
}