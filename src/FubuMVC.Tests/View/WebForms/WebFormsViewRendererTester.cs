using System;
using System.Web.UI;
using NUnit.Framework;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Routing;
using FubuMVC.Core.View;
using FubuMVC.Core.View.WebForms;
using Rhino.Mocks;

namespace FubuMVC.Tests.View.WebForms
{
    [TestFixture]
    public class WebFormsViewRendererTester
    {
        private WebFormsViewRenderer _renderer;
        private IWebFormsControlBuilder _builder;
        private TestOutputModel _outputModel;
        private TestControl _control;
        private string _expectedVirtualPath;
        private string _expectedRendering;
        private Type _expectedType;
        private DefaultControllerConfigContext _context;
        private FubuConventions _conventions;

        [SetUp]
        public void SetUp()
        {
            _outputModel = new TestOutputModel();

            _control = new TestControl();
            _builder = MockRepository.GenerateMock<IWebFormsControlBuilder>();
            _conventions = new FubuConventions();
            _context = new DefaultControllerConfigContext();
            _renderer = new WebFormsViewRenderer(_conventions, _context, _builder);

            _expectedType = typeof(IFubuViewWithModel);

            _expectedVirtualPath = "EXPECTED";
            _expectedRendering = "EXPECTED RENDER";

            _conventions.DefaultPathToViewForAction = config => _expectedVirtualPath;

            _builder.Stub(b => b.LoadControlFromVirtualPath(_expectedVirtualPath, _expectedType)).Return(_control);
            _builder.Stub(b => b.ExecuteControl(_control)).Return(_expectedRendering);
        }

        [Test]
        public void should_use_selector_strategy_to_get_view_path()
        {
            _renderer.RenderView(_outputModel);
            _builder.AssertWasCalled(b => b.LoadControlFromVirtualPath(_expectedVirtualPath, _expectedType));
        }

        [Test]
        public void should_set_up_viewmodel_on_page_if_present()
        {
            _renderer.RenderView(_outputModel);
            _control.Model.ShouldBeTheSameAs(_outputModel);
        }

        [Test]
        public void should_execute_control()
        {
            _renderer.RenderView(_outputModel);

            _builder.AssertWasCalled(b => b.ExecuteControl(_control));
        }

        [Test]
        public void should_return_render_output()
        {
            _renderer.RenderView(_outputModel).ShouldEqual(_expectedRendering);
        }

        private class TestControl : Control, IFubuView<TestOutputModel>
        {
            public TestOutputModel Model { get; set; }
            public void SetModel(object model)
            {
                Model = (TestOutputModel)model;
            }
        }
    }
}
