using NUnit.Framework;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Controller;
using FubuMVC.Core.Controller.Results;
using Rhino.Mocks;

namespace FubuMVC.Tests.Behaviors
{
    [TestFixture]
    public class OutputAsJsonTester : BehaviorTestContext<OutputAsJson>
    {
        protected override OutputAsJson CreateBehavior()
        {
            var behavior = base.CreateBehavior();
            behavior.InsideBehavior = new DefaultBehavior();
            return behavior;
        }

        [Test]
        public void should_set_the_result_to_return_json_when_it_cannot_be_determined_if_the_request_was_an_ajax_call()
        {
            _behavior.Invoke(_input, i => new TestOutputModel());
            _behavior.Result.ShouldBeOfType<RenderJsonResult<TestOutputModel>>();
        }

        [Test]
        public void should_respect_result_override()
        {
            var result = MockRepository.GenerateStub<IInvocationResult>();

            _behavior.Invoke(_input, i => new OverrideModel { ResultOverride = result });
            _behavior.Result.ShouldBeTheSameAs(result);
        }

        [Test]
        public void should_set_the_result_to_return_html_friendly_json_if_the_request_was_not_an_ajax_call()
        {
            _input = new TestAjaxRequestInputModel();
            _behavior.Invoke(_input, i => new TestOutputModel());
            _behavior.Result.ShouldBeOfType<RenderHtmlFriendlyJsonResult<TestOutputModel>>();
        }

        [Test]
        public void should_set_the_result_to_return_json_if_the_request_is_detected_as_an_ajax_call()
        {
            _input = new TestAjaxRequestInputModel { HTTP_X_REQUESTED_WITH = "XMLHttpRequest"};
            _behavior.Invoke(_input, i => new TestOutputModel());
            _behavior.Result.ShouldBeOfType<RenderJsonResult<TestOutputModel>>();
        }


        public class TestAjaxRequestInputModel : ICanDetectAjaxRequests
        {
            public string HTTP_X_REQUESTED_WITH
            {
                get; set;
            }
        }
    }
}