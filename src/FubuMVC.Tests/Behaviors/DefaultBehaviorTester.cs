using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Controller;
using FubuMVC.Core.Controller.Results;
using FubuMVC.Core.Routing;
using FubuMVC.Core.View;
using Rhino.Mocks;

namespace FubuMVC.Tests.Behaviors
{
    [TestFixture]
    public class DefaultBehaviorTester : BehaviorTestContext<DefaultBehavior>
    {
        [Test]
        public void should_execute_action_func_and_return()
        {
            var expected = new TestOutputModel();
            _behavior.Invoke(_input, i => expected).ShouldBeTheSameAs(expected);
        }

        [Test]
        public void should_set_the_result_to_render_a_view()
        {
            _behavior.Invoke(_input, i => new TestOutputModel());
            _behavior.Result.ShouldBeOfType<RenderViewResult<TestOutputModel>>();
        }

        [Test]
        public void should_set_normal_result_if_override_is_null()
        {
            _behavior.Invoke(_input, i => new OverrideModel());
            _behavior.Result.ShouldBeOfType<RenderViewResult<OverrideModel>>();
        }

        [Test]
        public void should_respect_result_override()
        {
            var result = MockRepository.GenerateStub<IInvocationResult>();

            _behavior.Invoke(_input, i => new OverrideModel{ResultOverride = result});
            _behavior.Result.ShouldBeTheSameAs(result);
        }
    }

    public class OverrideModel : ISupportResultOverride
    {
        public IInvocationResult ResultOverride { get; set; }
    }
}