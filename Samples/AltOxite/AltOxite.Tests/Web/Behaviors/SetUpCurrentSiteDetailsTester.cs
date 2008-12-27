using AltOxite.Core.Domain;
using AltOxite.Core.Web.Behaviors;
using NUnit.Framework;

namespace AltOxite.Tests.Web.Behaviors
{
    [TestFixture]
    public class SetUpCurrentSiteDetailsTester
    {
        private set_the_current_site_details_on_the_output_viewmodel _behavior;
        private SiteConfiguration _config;
        private TestViewModel _model;

        [SetUp]
        public void SetUp()
        {
            _config = new SiteConfiguration();
            _model = new TestViewModel();
            _behavior = new set_the_current_site_details_on_the_output_viewmodel(_config);
        }

        [Test]
        public void ModifyOutput_override_should_modify_the_output()
        {
            _behavior.ModifyOutput(_model);
            _model.SiteConfig.ShouldBeTheSameAs(_config);
        }
    }
}