using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using FubuMVC.Core;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Html;
using NUnit.Framework;

namespace FubuMVC.Tests.Controller.Config
{
    [TestFixture]
    public class LocalizationTester
    {
        private CultureInfo _currentUICulture;

        [SetUp]
        public void SetUp()
        {
            _currentUICulture = Thread.CurrentThread.CurrentUICulture;
            // Force the current thread to English, in case
            // these tests are run on a machine with a different default OS language.
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        }

        [TearDown]
        public void TearDown()
        {
            Thread.CurrentThread.CurrentUICulture = _currentUICulture;
        }

        [Test]
        public void configure_sets_the_current_ui_culture()
        {
            new Localization().Configure(new Dictionary<string, object>() { { "HTTP_ACCEPT_LANGUAGE", "fr-FR" } });
            Thread.CurrentThread.CurrentUICulture.ToString().ShouldBeEqualIgnoringCase("fr-FR");
        }

        [Test]
        public void configure_does_nothing_for_no_input()
        {
            new Localization().Configure(new Dictionary<string, object>() { { "HTTP_ACCEPT_LANGUAGE", "" } });
            Thread.CurrentThread.CurrentUICulture.ToString().ShouldBeEqualIgnoringCase("en-US");
        }

        [Test]
        public void configure_does_nothing_for_invalid_input()
        {
            new Localization().Configure(new Dictionary<string, object>() { { "HTTP_ACCEPT_LANGUAGE", "asdf" } });
            Thread.CurrentThread.CurrentUICulture.ToString().ShouldBeEqualIgnoringCase("en-US");
        }
    }
}
