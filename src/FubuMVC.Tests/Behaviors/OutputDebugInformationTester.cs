using System;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Controller;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Controller.Results;
using FubuMVC.Core.Conventions.ControllerActions;
using FubuMVC.Core.Routing;
using NUnit.Framework;
using Rhino.Mocks;

namespace FubuMVC.Tests.Behaviors
{
    [TestFixture]
    public class OutputDebugInformationTester
    {
        private OutputDebugInformation _behavior;
        private ICurrentRequest _currentRequest;
        private TestOutputModel _outputModel;

        [SetUp]
        public void SetUp()
        {
            _outputModel = new TestOutputModel();
            _currentRequest = MockRepository.GenerateStub<ICurrentRequest>();
            _behavior = new OutputDebugInformation(_currentRequest, new FubuConventions(), new FubuConfiguration(new FubuConventions()))
            {
                InsideBehavior = new DefaultBehavior()
            };
        }

        [Test]
        public void should_respect_result_override()
        {
            var result = MockRepository.GenerateStub<IInvocationResult>();

            _behavior.Invoke(new TestInputModel(), i => new OverrideModel { ResultOverride = result });
            _behavior.Result.ShouldBeTheSameAs(result);
        }

        [Test]
        public void should_not_do_anything_when_the_url_does_not_contain_the_debug_url()
        {
            _behavior.Invoke(new TestInputModel(), i => _outputModel);
            _behavior.Result.ShouldBeOfType<RenderViewResult<TestOutputModel>>();
        }

        [Test]
        public void should_render()
        {
            _currentRequest.Expect(x => x.GetUrl()).Return(new Uri("http://{0}".ToFormat(wire_up_debug_handler_URL.DEBUG_URL)));

            _behavior.Invoke(new TestInputModel(), i => _outputModel);
            _behavior.Result.ShouldBeOfType<RenderDebugInformationResult>();
        }
    }
}