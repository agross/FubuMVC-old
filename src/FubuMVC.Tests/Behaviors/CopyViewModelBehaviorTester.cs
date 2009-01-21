using FubuMVC.Core.Behaviors;
using NUnit.Framework;

namespace FubuMVC.Tests.Behaviors
{
    [TestFixture]
    public class CopyViewModelBehaviorTester
    {
        private copy_viewmodel_from_input_to_output<TestViewModel> _behavior;

        [SetUp]
        public void SetUp()
        {
            _behavior = new copy_viewmodel_from_input_to_output<TestViewModel>();
        }

        [Test]
        public void verify_that_all_properties_are_being_copied()
        {
            TestViewModel input = new TestViewModel{ BoolProperty = true, IntProperty = 10, StringProperty = "test" };
            TestViewModel output = new TestViewModel();
            _behavior.PrepareInput(input);
            _behavior.ModifyOutput(output);

            output.BoolProperty.ShouldEqual(input.BoolProperty);
            output.IntProperty.ShouldEqual(input.IntProperty);
            output.StringProperty.ShouldEqual(input.StringProperty);
        }
    }

    public class TestViewModel
    {
        public bool BoolProperty { get; set; }
        public string StringProperty { get; set; }
        public int IntProperty { get; set; }
    }
}