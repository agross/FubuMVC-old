using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using FubuMVC.Core.Controller.Results;
using FubuMVC.Core.Routing;
using Rhino.Mocks;

namespace FubuMVC.Tests.Controller.Results
{
    [TestFixture]
    public class RenderHtmlFriendlyJsonResultTester
    {
        private IServiceLocator _serviceLocator;
        private IOutputWriter _outputWriter;
        private RenderJsonResult<TestOutputModel> _result;
        private SpecificationExtensions.CapturingConstraint writerArguments;

        [SetUp]
        public void Setup()
        {
            _serviceLocator = MockRepository.GenerateMock<IServiceLocator>();
            _outputWriter = MockRepository.GenerateMock<IOutputWriter>();
            _serviceLocator.Stub(s => s.GetInstance<IOutputWriter>()).Return(_outputWriter);

            _result = new RenderHtmlFriendlyJsonResult<TestOutputModel>(new TestOutputModel { Prop1 = "PropValue" });

            writerArguments = _outputWriter.CaptureArgumentsFor(w => w.Write(null, null));
            _result.Execute(_serviceLocator);
        }

        [Test]
        public void should_render_using_html_content_type()
        {
            writerArguments.First<string>().ShouldEqual("text/html");
        }

        [Test]
        public void should_return_the_json_within_a_textarea()
        {
            writerArguments.Second<string>().ShouldContain("<textarea>{\"Prop1\":\"PropValue\"}</textarea>");
        }
    }
}
