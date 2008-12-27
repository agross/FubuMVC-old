using System;
using System.Linq;
using FubuMVC.Core.Controller.Config.DSL;
using FubuMVC.Tests.Controller.Config.DSL.FubuMVC.Tests.Controller.Config.DSL.ControllerAutoDiscoveryTests;
using NUnit.Framework;

namespace FubuMVC.Tests.Controller.Config.DSL
{
    [TestFixture]
    public class ControllerTypeScanningExpressionTester_type_scanning_tests
    {
        private AutoControllerConfiguration _autoConfig;
        private ControllerTypeScanningExpression _expression;

        [SetUp]
        public void SetUp()
        {
            _autoConfig = new AutoControllerConfiguration();
            _expression = new ControllerTypeScanningExpression(_autoConfig, typeof (TestController).Assembly);
        }

        [Test]
        public void Where_should_filter_out_unmatched_types()
        {
            _expression.Where(t => 
                t.Name == "TestController");

            _autoConfig.GetDiscoveredTypes().Single().ShouldEqual(typeof (TestController));
        }

        [Test]
        public void should_be_able_to_filter_on_namespaces()
        {
            _expression.Where(t => 
                t.Namespace.EndsWith("ControllerAutoDiscoveryTests"));

            _autoConfig.GetDiscoveredTypes().ShouldHaveTheSameElementsAs(
                typeof(AutoDiscoveredWithBase1),
                typeof(AutoDiscoveredController2),
                typeof(AutoDiscoveredWithAttribute3));
        }

        [Test]
        public void should_be_able_to_filter_on_type_name()
        {
            _expression.Where(t => 
                t.Namespace.EndsWith("ControllerAutoDiscoveryTests") 
                && t.Name.ToLowerInvariant().Contains("controller"));

            _autoConfig.GetDiscoveredTypes().Single().ShouldEqual(typeof (AutoDiscoveredController2));
        }
        
        [Test]
        public void should_be_able_to_filter_on_type_attributes()
        {
            _expression.Where(t =>
                t.GetCustomAttributes(typeof(AutoDiscoverCustomAttribute), true).Count() > 0);

            _autoConfig.GetDiscoveredTypes().Single().ShouldEqual(typeof(AutoDiscoveredWithAttribute3));
        }

        [Test]
        public void should_not_include_abstract_types_by_default()
        {
            _expression.Where(t => t.Equals(typeof(AutoDiscoveredBaseClass)));

            _autoConfig.GetDiscoveredTypes().ShouldHaveCount(0);
        }

        [Test]
        public void should_not_include_value_types_by_default()
        {
            _expression.Where(t => t.Equals(typeof(AutoDiscoveredStruct)));

            _autoConfig.GetDiscoveredTypes().ShouldHaveCount(0);
        }

        [Test]
        public void should_not_include_static_classes_by_default()
        {
            _expression.Where(t => t.Equals(typeof(AutoDiscoveredStaticClass)));

            _autoConfig.GetDiscoveredTypes().ShouldHaveCount(0);
        }

        [Test]
        public void should_not_include_interfaces_by_default()
        {
            _expression.Where(t => t.Equals(typeof(IAutoDiscovered)));

            _autoConfig.GetDiscoveredTypes().ShouldHaveCount(0);
        }

        [Test]
        public void should_be_able_to_filter_on_base_types()
        {
            _expression.Where(t =>
                typeof(AutoDiscoveredBaseClass).IsAssignableFrom(t));

            _autoConfig.GetDiscoveredTypes().Single().ShouldEqual(typeof (AutoDiscoveredWithBase1));
        }
    }

    [TestFixture]
    public class ControllerTypeScanningExpressionTester_method_scanning_tests
    {
        private AutoControllerConfiguration _autoConfig;
        private ControllerTypeScanningExpression _expression;

        [SetUp]
        public void SetUp()
        {
            _autoConfig = new AutoControllerConfiguration();
            _expression = new ControllerTypeScanningExpression(_autoConfig, typeof(TestController).Assembly);
            _expression.Where(t => t == typeof(AutoDiscoverWithActions));
            _expression.MapActionsWhere((m, i, o) => true);
        }

        [Test]
        public void should_ignore_methods_that_are_not_onemodelinonemodelout()
        {
            _autoConfig.GetDiscoveredActions().SingleOrDefault(m => 
                m.Action.Name.Equals("NonOMIOMOMethod")
                || m.Action.Name.Equals("NonOMIOMOMethod2")).ShouldBeNull();
        }

        [Test]
        public void should_ignore_methods_whose_input_or_output_types_are_not_classes()
        {
            _autoConfig.GetDiscoveredActions().SingleOrDefault(m =>
                m.Action.Name.Equals("MethodWithNonClassInput")
                || m.Action.Name.Equals("MethodWithNonClassOutput")).ShouldBeNull();
        }

        [Test]
        public void should_add_public_methods()
        {
            _autoConfig.GetDiscoveredActions().SingleOrDefault(m => m.Action.Name.Equals("NormalAction")).ShouldNotBeNull();
        }

        [Test]
        public void should_ignore_non_public_methods()
        {
            _autoConfig.GetDiscoveredActions().SingleOrDefault(m => 
                m.Action.Name.Equals("ProtectedBaseAction")
                || m.Action.Name.Equals("PrivateMethod")
                || m.Action.Name.Equals("InternalMethod")
                || m.Action.Name.Equals("ProtectedInternalMethod")
                || m.Action.Name.Equals("defaultAccessMethod")
                ).ShouldBeNull();
        }

        [Test]
        public void should_see_inherited_methods()
        {
            _autoConfig.GetDiscoveredActions().SingleOrDefault(m => m.Action.Name.Equals("AbstractBaseAction"))
                .ShouldNotBeNull();
        }

        [Test]
        public void should_see_virtual_base_methods_that_are_not_overriden()
        {
            _autoConfig.GetDiscoveredActions().SingleOrDefault(m => m.Action.Name.Equals("VirtualBaseAction"))
                .ShouldNotBeNull();
        }

        [Test]
        public void should_see_non_virtual_base_methods()
        {
            _autoConfig.GetDiscoveredActions().SingleOrDefault(m => m.Action.Name.Equals("NonVirtBaseAction"))
                .ShouldNotBeNull();
        }

        [Test]
        public void should_not_see_properties()
        {
            _autoConfig.GetDiscoveredActions().SingleOrDefault(m => m.Action.Name.Contains("TestProperty"))
                .ShouldBeNull();
        }

        [Test]
        public void should_not_see_generic_methods()
        {
            _autoConfig.GetDiscoveredActions().SingleOrDefault(m=>m.Action.Name.Equals("GenericMethod")).ShouldBeNull();
        }
    }

    public class AutoDiscoverCustomAttribute : Attribute { }

    public abstract class AutoDiscoverWithActionsBase
    {
        public abstract TestOutputModel AbstractBaseAction(TestInputModel input);
        public virtual TestOutputModel VirtualBaseAction(TestInputModel input)
        {
            return new TestOutputModel();
        }
        public TestOutputModel NonVirtBaseAction(TestInputModel input)
        {
            return new TestOutputModel();
        }

        protected TestOutputModel ProtectedBaseAction(TestInputModel input)
        {
            return new TestOutputModel();
        }
    }

    public class AutoDiscoverWithActions : AutoDiscoverWithActionsBase
    {
        public override TestOutputModel AbstractBaseAction(TestInputModel input)
        {
            return new TestOutputModel();
        }

        public TestOutputModel NormalAction(TestInputModel input)
        {
            return new TestOutputModel();
        }

        public TestOutputModel GenericMethod<T>(TestInputModel input)
        {
            return new TestOutputModel();
        }

        public TestOutputModel TestProperty { get; set; }

        private TestOutputModel PrivateMethod(TestInputModel inputModel) 
        {
            return new TestOutputModel();
        }

        internal TestOutputModel InternalMethod(TestInputModel  inputModel)
        {
            return new TestOutputModel();
        }

        protected internal TestOutputModel ProtectedInternalMethod(TestInputModel inputModel)
        {
            return new TestOutputModel();
        }

        TestOutputModel defaultAccessMethod(TestInputModel inputModel)
        {
            return new TestOutputModel();
        }

        public void NonOMIOMOMethod(){}
        
        public TestOutputModel NonOMIOMOMethod2(TestInputModel input, string bogus) 
        {
            return new TestOutputModel(); 
        }

        public int MethodWithNonClassOutput(TestInputModel input)
        {
            return 0;
        }

        public TestOutputModel MethodWithNonClassInput(int blah)
        {
            return new TestOutputModel();
        }
    }

    namespace FubuMVC.Tests.Controller.Config.DSL.ControllerAutoDiscoveryTests
    {
        public struct AutoDiscoveredStruct{}
        public static class AutoDiscoveredStaticClass{}
        public abstract class AutoDiscoveredBaseClass { }
        public interface IAutoDiscovered{}
        public class AutoDiscoveredWithBase1 : AutoDiscoveredBaseClass{ }
        public class AutoDiscoveredController2{ }
        
        [AutoDiscoverCustom]
        public class AutoDiscoveredWithAttribute3{ }
    }
}