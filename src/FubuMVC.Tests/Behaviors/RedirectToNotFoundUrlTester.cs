using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Controller.Results;
using NUnit.Framework;

namespace FubuMVC.Tests.Behaviors
{
    [TestFixture]
    public class when_hitting_an_unbound_url
    {
        private RedirectToNotFoundUrl _behavior;
        private TestOutputModel _outputModel;
        private FubuConventions _conventions;

        [SetUp]
        public void SetUp()
        {
            _outputModel = new TestOutputModel();
            _conventions = new FubuConventions();
            _behavior = new RedirectToNotFoundUrl(_conventions)
            {
                InsideBehavior = new DefaultBehavior()
            };
        }

        [Test]
        public void should_redirect_to_not_found_url()
        {
            _behavior.Invoke(new TestInputModel(), i => _outputModel);
            _behavior.Result.ShouldBeOfType<RedirectResult>().Url.ShouldEqual(_conventions.PageNotFoundUrl);
        }
    }
}