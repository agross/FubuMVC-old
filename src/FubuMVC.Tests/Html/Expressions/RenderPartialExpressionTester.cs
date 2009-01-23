using FubuMVC.Core;
using NUnit.Framework;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Html.Expressions;
using FubuMVC.Core.View;
using FubuMVC.Core.View.WebForms;
using Rhino.Mocks;

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
        private FubuConventions _conventions;

        [SetUp]
        public void SetUp()
        {
            _conventions = new FubuConventions();

            _view = MockRepository.GenerateStub<IFubuView<TestModel>>();
            _renderer = MockRepository.GenerateStub<IWebFormsViewRenderer>();

            _model = new TestModel();
            _partialModel = new PartialTestModel();

            _model.PartialModel = _partialModel;

            _view.Stub(v => v.Model).Return(_model);

            _forExpression = new RenderPartialExpression(_view, _renderer, _conventions)
                                .Using<TestUserControl>();
        }

        [Test]
        public void a_call_to_For_should_result_in_only_one_render()
        {
            _renderer.Expect(r => r.RenderView<TestUserControl>(null, null, null)).IgnoreArguments().Return("").Repeat.Once();

            _forExpression.For<TestModel, PartialTestModel>(m => m.PartialModel);

            _renderer.VerifyAllExpectations();
        }

        [Test]
        public void a_call_to_For_should_pass_the_correct_model_to_render()
        {
            var expectedResult = "FOO";
            var catcher = _renderer.CaptureArgumentsFor(r => r.RenderView<TestUserControl>(null, null, null), o => o.Return(expectedResult));

            _forExpression.For<TestModel, PartialTestModel>(m => m.PartialModel).ShouldEqual(expectedResult);

            catcher.Second<PartialTestModel>().ShouldBeTheSameAs(_partialModel);
        }

        [Test]
        public void a_call_to_For_with_null_and_WithDefault_should_reneder_default_value()
        {
            var expectedResult = "FOO";

            new RenderPartialExpression(_view, _renderer, _conventions)
                    .Using<TestUserControl>()
                    .WithDefault(expectedResult)
                    .For(null as object)
                    .ShouldEqual(expectedResult);
        }

        [Test]
        public void a_call_to_ForEachOf_with_empty_list_and_WithDefault_should_reneder_default_value()
        {
            var expectedResult = "FOO";

            new RenderPartialExpression(_view, _renderer, _conventions)
                    .Using<TestUserControl>()
                    .WithDefault(expectedResult)
                    .ForEachOf(new object[]{})
                    .ShouldEqual(expectedResult);
        }

        [Test]
        public void should_use_conventions_to_locate_partial_view_file()
        {
            var expectedViewFile = "FOO";
            _conventions.DefaultPathToPartialView = t => expectedViewFile;

            var catcher = _renderer.CaptureArgumentsFor(r => r.RenderView<TestUserControl>(null, null, null), o => o.Return(""));

            _forExpression.For<TestModel, PartialTestModel>(m => m.PartialModel).ShouldEqual("");

            catcher.First<string>().ShouldEqual(expectedViewFile);
        }

        [Test]
        public void should_use_conventions_to_render_ForEachOf_header()
        {
            var expectedBeginning = "BEGINNING";

            _conventions.PartialForEachOfHeader = (model, total) => new GenericOpenTagExpression(expectedBeginning);

            _forExpression.ForEachOf(new[] {_partialModel}).ShouldStartWith("<BEGINNING>");
        }

        [Test]
        public void should_use_conventions_to_render_ForEachOf_Footer()
        {
            var expectedEnding = "ENDING";

            _conventions.PartialForEachOfFooter = (model, total) => expectedEnding;

            _forExpression.ForEachOf(new[] { _partialModel }).ShouldEndWith("ENDING");
        }

        [Test]
        public void should_use_conventions_to_render_pre_and_post_item()
        {
            var expectedBefore = "START";
            var expectedAfter = "END";
            var expectedItem = "ITEM";

            _renderer.Stub(r => r.RenderView<TestUserControl>(null, null, null)).IgnoreArguments().Return(expectedItem);

            _conventions.PartialForEachOfBeforeEachItem = (model, index, total) => new GenericOpenTagExpression(expectedBefore);
            _conventions.PartialForEachOfAfterEachItem = (model, index, total) => expectedAfter;

            _forExpression.ForEachOf(new[] { _partialModel }).ShouldContain("<{0}>{1}{2}".ToFormat(expectedBefore, expectedItem, expectedAfter));
        }

        [Test]
        public void should_skip_conventions_to_render_header_and_footer_item()
        {
            var before = "START";
            var after = "END";
            var item = "ITEM";
            var beginning = "BEGINNING";
            var ending = "ENDING";

            _renderer.Stub(r => r.RenderView<TestUserControl>(null, null, null)).IgnoreArguments().Return(item);

            _conventions.PartialForEachOfHeader = (model, total) => new GenericOpenTagExpression(beginning);
            _conventions.PartialForEachOfBeforeEachItem = (model, index, total) => new GenericOpenTagExpression(before);
            _conventions.PartialForEachOfAfterEachItem = (model, index, total) => after;
            _conventions.PartialForEachOfFooter = (model, total) => ending;

            new RenderPartialExpression(_view, _renderer, _conventions)
                .Using<TestUserControl>()
                .WithoutListWrapper()
                .ForEachOf(new[] { _partialModel })
                .ShouldEqual("<{0}>{1}{2}".ToFormat(before, item, after));
        }

        [Test]
        public void should_skip_conventions_to_render_pre_and_post_item()
        {
            var before = "START";
            var after = "END";
            var item = "ITEM";
            var beginning = "BEGINNING";
            var ending = "ENDING";

            _renderer.Stub(r => r.RenderView<TestUserControl>(null, null, null)).IgnoreArguments().Return(item);

            _conventions.PartialForEachOfHeader = (model, total) => new GenericOpenTagExpression(beginning);
            _conventions.PartialForEachOfBeforeEachItem = (model, index, total) => new GenericOpenTagExpression(before);
            _conventions.PartialForEachOfAfterEachItem = (model, index, total) => after;
            _conventions.PartialForEachOfFooter = (model, total) => ending;

            new RenderPartialExpression(_view, _renderer, _conventions)
                .Using<TestUserControl>()
                .WithoutItemWrapper()
                .ForEachOf(new[] { _partialModel })
                .ShouldEqual("<{0}>{1}{2}".ToFormat(beginning, item, ending));
        }

        [Test]
        public void should_skip_all_conventions_for_item()
        {
            var before = "START";
            var after = "END";
            var item = "ITEM";
            var beginning = "BEGINNING";
            var ending = "ENDING";

            _renderer.Stub(r => r.RenderView<TestUserControl>(null, null, null)).IgnoreArguments().Return(item);

            _conventions.PartialForEachOfHeader = (model, total) => new GenericOpenTagExpression(beginning);
            _conventions.PartialForEachOfBeforeEachItem = (model, index, total) => new GenericOpenTagExpression(before);
            _conventions.PartialForEachOfAfterEachItem = (model, index, total) => after;
            _conventions.PartialForEachOfFooter = (model, total) => ending;

            new RenderPartialExpression(_view, _renderer, _conventions)
                .Using<TestUserControl>()
                .WithoutListWrapper()
                .WithoutItemWrapper()
                .ForEachOf(new[] { _partialModel })
                .ShouldEqual("{0}".ToFormat(item));
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
