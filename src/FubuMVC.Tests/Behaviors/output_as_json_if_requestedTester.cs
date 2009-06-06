using System;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Controller;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Controller.Results;
using FubuMVC.Core.Routing;
using NUnit.Framework;
using Rhino.Mocks;

namespace FubuMVC.Tests.Behaviors
{
    [TestFixture]
    public class when_requesting_an_action_with_json_url : BehaviorTestContext<output_as_json_if_requested>
    {
        private FubuConventions _conventions;
        private ICurrentRequest _request;

        protected override output_as_json_if_requested CreateBehavior()
        {
            _request = MockRepository.GenerateStub<ICurrentRequest>();
            _conventions = new FubuConventions();
            _request.Stub(r => r.GetUrl()).Return(new Uri("http://localhost/foo" + _conventions.DefaultJsonExtension));
            var behavior = new output_as_json_if_requested(_request, _conventions)
                {
                    InsideBehavior = new DefaultBehavior()
                };
            return behavior;
        }

        [Test]
        public void should_set_the_result_to_return_json()
        {
            _behavior.Invoke(_input, i => new TestOutputModel());
            _behavior.Result.ShouldBeOfType<RenderJsonResult<TestOutputModel>>();
        }
    }

    [TestFixture]
    public class when_requesting_an_action_with_non_json_url : BehaviorTestContext<output_as_json_if_requested>
    {
        private FubuConventions _conventions;
        private ICurrentRequest _request;

        protected override output_as_json_if_requested CreateBehavior()
        {
            _request = MockRepository.GenerateStub<ICurrentRequest>();
            _conventions = new FubuConventions();
            _request.Stub(r => r.GetUrl()).Return(new Uri("http://localhost/foo"));
            var behavior = new output_as_json_if_requested(_request, _conventions)
                {
                    InsideBehavior = new DefaultBehavior()
                };
            return behavior;
        }

        [Test]
        public void should_not_tamper_with_result()
        {
            
            _behavior.Invoke(_input, i => new TestOutputModel());
            _behavior.Result.ShouldNotBeOfType(typeof(RenderJsonResult<TestOutputModel>));
        }

        [Test]
        public void should_respect_result_override()
        {
            var result = MockRepository.GenerateStub<IInvocationResult>();

            _behavior.Invoke(_input, i => new OverrideModel { ResultOverride = result });
            _behavior.Result.ShouldBeTheSameAs(result);
        }
    }
}