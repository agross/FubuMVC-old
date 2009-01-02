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
            _config.LanguageDefault = "fr-FR";
            _config.SEORobots = "robots";
            _config.Name = "test";

            _behavior.ModifyOutput(_model);
            _model.LanguageDefault.ShouldBeTheSameAs(_config.LanguageDefault);
            _model.SEORobots.ShouldBeTheSameAs(_config.SEORobots);
            _model.SiteName.ShouldBeTheSameAs(_config.Name);
        }
    }
}