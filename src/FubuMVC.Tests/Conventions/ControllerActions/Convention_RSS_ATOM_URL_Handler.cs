using System.Linq;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Conventions.ControllerActions;
using FubuMVC.Core.Util;
using NUnit.Framework;

namespace FubuMVC.Tests.Conventions.ControllerActions
{
    [TestFixture]
    public class Convention_RSS_ATOM_URL_Handler
    {
        private string expectedRssExtension;
        private string expectedAtomExtension;
        private FubuConventions fubuConventions;
        private wire_up_RSS_and_ATOM_URLs_if_required convention;
        private ControllerActionConfig config;

        [SetUp]
        public void SetUp()
        {
            expectedRssExtension = "__EXPECTED_RSS__";
            expectedAtomExtension = "__EXPECTED_ATOM__";
            fubuConventions = new FubuConventions { DefaultRssExtension = expectedRssExtension, DefaultAtomExtension = expectedAtomExtension};
            convention = new wire_up_RSS_and_ATOM_URLs_if_required {FubuConventions = fubuConventions};
            var method = ReflectionHelper.GetMethod<TestController>(c => c.SomeAction(null));
            config = new ControllerActionConfig(method, null, null);
        }

        [Test]
        public void should_not_apply_if_output_as_rss_or_atom_behavior_not_defined()
        {
            convention.Apply(config);

            config.GetOtherUrls().ShouldHaveCount(0);
        }

        [Test]
        public void should_only_apply_if_output_as_rss_or_atom_behavior_defined()
        {
            config.AddBehavior<OutputAsRssOrAtomFeed>();

            convention.Apply(config);

            var urls = config.GetOtherUrls().ToArray();
            urls.ShouldHaveCount(2);
            urls[0].EndsWith(expectedRssExtension);
            urls[1].EndsWith(expectedAtomExtension);
        }
    }
}