using FubuMVC.Core.Behaviors;
using NUnit.Framework;

namespace FubuMVC.Tests.Behaviors
{
    [TestFixture]
    public class CopyViewModelBehaviorTester
    {
        [Test]
        public void verify_that_all_properties_are_being_copied()
        {
            copy_viewmodel_from_input_to_output<TestViewModel>  _behavior = new copy_viewmodel_from_input_to_output<TestViewModel>();
            TestViewModel input = new TestViewModel { BoolProperty = true, IntProperty = 10, StringProperty = "test" };
            TestViewModel output = new TestViewModel();
            _behavior.PrepareInput(input);
            _behavior.ModifyOutput(output);

            output.BoolProperty.ShouldEqual(input.BoolProperty);
            output.IntProperty.ShouldEqual(input.IntProperty);
            output.StringProperty.ShouldEqual(input.StringProperty);
        }

        [Test]
        public void verify_that_only_read_and_write_properties_are_being_copied()
        {
            copy_viewmodel_from_input_to_output<OtherTestViewModel> _behavior = new copy_viewmodel_from_input_to_output<OtherTestViewModel>();
            OtherTestViewModel input = new OtherTestViewModel("test", "test", "test");
            OtherTestViewModel output = new OtherTestViewModel();
            _behavior.PrepareInput(input);
            _behavior.ModifyOutput(output);

            output.StringProperty1.ShouldNotEqual(input.StringProperty1);
            output.GetStringProperty2().ShouldNotEqual(input.GetStringProperty2());
            output.StringProperty3.ShouldEqual(input.StringProperty3);
        }
    }

    public class TestViewModel
    {
        public bool BoolProperty { get; set; }
        public string StringProperty { get; set; }
        public int IntProperty { get; set; }
    }
    public class OtherTestViewModel
    {
        public OtherTestViewModel() {}
        public OtherTestViewModel(string stringProperty1, string stringProperty2, string stringProperty3)
        {
            StringProperty1 = stringProperty1;
            StringProperty2 = stringProperty2;
            StringProperty3 = stringProperty3;
        }
        public string StringProperty1 { get; private set; }
        public string StringProperty2 { private get; set; }
        public string StringProperty3 { get; set; }

        public string GetStringProperty2()
        {
            return StringProperty2;
        }
    }
}