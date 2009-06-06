using FubuMVC.Core.Controller.Results;
using NUnit.Framework;

namespace FubuMVC.Tests.Results
{
    [TestFixture]
    public class when_overriding_with_no_current_override
    {
        [Test]
        public void should_set_the_OverrideResult_property()
        {
            var overrider = new CurrentRequestResultOverride();
            var redirectResult = new RedirectResult("");
            overrider.OverrideIfNotAlreadyOverriden(redirectResult);

            overrider.OverrideResult.ShouldBeTheSameAs(redirectResult);
        }
    }

    [TestFixture]
    public class when_overriding_with_existing_current_override
    {
        [Test]
        public void should_leave_previous_override_in_place()
        {
            var overrider = new CurrentRequestResultOverride();
            var firstResult = new RedirectResult("");
            var secondResult = new RedirectResult("");
            overrider.OverrideIfNotAlreadyOverriden(firstResult);
            overrider.OverrideIfNotAlreadyOverriden(secondResult);

            overrider.OverrideResult.ShouldBeTheSameAs(firstResult);
        }
    }
}