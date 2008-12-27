using NUnit.Framework;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Html.Expressions;
using FubuMVC.Core.View;
using FubuMVC.Core.View.WebForms;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;

namespace FubuMVC.Tests.Html.Expressions
{
    [TestFixture]
    public class RenderPartialExpressionTester
    {
        private IFubuView<TestModel> _view;
        private IRenderPartialForScope _forExpression;
        private IWebFormsViewRenderer _renderer;
        private TestModel _model;
        private PartialTestModel _partialModel;

        [SetUp]
        public void SetUp()
        {
            _view = MockRepository.GenerateStub<IFubuView<TestModel>>();
            _renderer = MockRepository.GenerateStub<IWebFormsViewRenderer>();

            _model = new TestModel();
            _partialModel = new PartialTestModel();

            _model.PartialModel = _partialModel;

            _view.Stub(v => v.Model).Return(_model);

            _forExpression = new RenderPartialExpression(_view, _renderer)
                                .Using<TestUserControl>();
        }

        [Test]
        public void a_call_to_For_should_result_in_only_one_render()
        {
            _renderer.Expect(r => r.RenderView<TestUserControl>(null, null)).IgnoreArguments().Return("");

            _forExpression.For<TestModel, PartialTestModel>(m => m.PartialModel);
        }

        [Test]
        public void a_call_to_For_should_pass_the_correct_model_to_render()
        {
            _renderer.Expect(r => r.RenderView<TestUserControl>(null, null)).Return("").Constraints(
                Is.Same(_partialModel),
                Is.NotNull());

            _forExpression.For<TestModel, PartialTestModel>(m => m.PartialModel);
        }

        [Test]
        public void a_call_to_For_should_pass_the_correct_prefix_to_render()
        {
            _renderer.Expect(r => r.RenderView<TestUserControl>(null, null)).Return("").Constraints(
                Is.Anything(),
                Is.Equal("PartialModel"));

            _forExpression.For<TestModel, PartialTestModel>(m => m.PartialModel);
        }

        public class TestModel
        {
            public PartialTestModel PartialModel { get; set; }
            public PartialTestModel[] PartialModelArray { get; set; }
        }

        public class PartialTestModel
        {

        }
    }
}
