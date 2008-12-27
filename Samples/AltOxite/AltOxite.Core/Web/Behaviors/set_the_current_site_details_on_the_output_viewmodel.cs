using AltOxite.Core.Domain;
using FubuMVC.Core.Behaviors;

namespace AltOxite.Core.Web.Behaviors
{
    public class set_the_current_site_details_on_the_output_viewmodel : behavior_base_for_convenience
    {
        private readonly SiteConfiguration _siteConfig;

        public set_the_current_site_details_on_the_output_viewmodel(SiteConfiguration config)
        {
            _siteConfig = config;
        }

        public void UpdateModel(ViewModel model)
        {
            if (model == null) return;

            model.SiteConfig = _siteConfig;
        }

        public override void ModifyOutput<OUTPUT>(OUTPUT output)
        {
            UpdateModel(output as ViewModel);
        }
    }
}