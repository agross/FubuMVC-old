using System;
using System.Web.UI;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Controller;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Conventions;
using FubuMVC.Core.View;

namespace FubuMVC.Tests
{
    public class TestController
    {
        public TestController()
        {
        }

        public TestController(IFubuConvention<TestController>[] conventions)
        {
            Conventions = conventions;
        }

        public IFubuConvention<TestController>[] Conventions { get; private set; }

        public TestOutputModel RequiredParamsAction(TestInputRequiredModel value)
        {
            return new TestOutputModel();
        }

        public TestOutputModel SomeAction(TestInputModel value)
        {
            return new TestOutputModel { Prop1 = value.Prop1 };
        }

        public TestOutputModel SomeAction(int not_used)
        {
            return new TestOutputModel();
        }

        public TestOutputModel2 AnotherAction(TestInputModel value)
        {
            return new TestOutputModel2 { Prop1 = value.Prop1 };
        }

        public TestOutputModel3 ThirdAction(TestInputModel value)
        {
            return new TestOutputModel3 { Prop1 = value.Prop1 };
        }

        public void RedirectAction(TestInputModel value)
        {
        }
    }

    public class TestInputModel
    {
        public int PropInt { get; set; }
        public string Prop1 { get; set; }
    }

    public class TestInputRequiredModel
    {
        public int PropInt { get; set; }
        [Required]
        public string Prop1 { get; set; }
        [Required]
        public string Prop3 { get; set; } //NOTE: Purposely out of order to test that the Url parameter stuff preserves declared ordering
        [Required]
        public string Prop2 { get; set; }
    }

    public class TestOutputModel
    {
        public string Prop1 { get; set; }
    }

    public class TestOutputModel2 : TestOutputModel
    {
    }

    public class TestOutputModel3 : TestOutputModel
    {
    }

    public class TestPartialModel
    {
        public string PartialModelProp1 { get; set; }
    }

    public class TestView : IFubuView<TestOutputModel>
    {
        public void SetModel(object model)
        {
            throw new System.NotImplementedException();
        }

        public TestOutputModel Model
        {
            get { throw new System.NotImplementedException(); }
        }
    }

    public class TestUserControl : UserControl, IFubuView<TestPartialModel>
    {
        public void SetModel(object model)
        {
            throw new System.NotImplementedException();
        }

        public TestPartialModel Model
        {
            get { throw new System.NotImplementedException(); }
        }
    }

    public class TestBehavior2 : IControllerActionBehavior
    {
        public IControllerActionBehavior InsideBehavior { get; set; }
        public IInvocationResult Result { get; set; }
        public OUTPUT Invoke<INPUT, OUTPUT>(INPUT input, Func<INPUT, OUTPUT> func)
            where INPUT : class
            where OUTPUT : class
        {
            throw new System.NotImplementedException();
        }
    }

    public class TestBehavior : IControllerActionBehavior
    {
        public IControllerActionBehavior InsideBehavior { get; set; }
        public IInvocationResult Result { get; set; }
        public OUTPUT Invoke<INPUT, OUTPUT>(INPUT input, Func<INPUT, OUTPUT> func)
            where INPUT : class
            where OUTPUT : class
        {
            throw new System.NotImplementedException();
        }
    }

    public class TestControllerConvention : IFubuConvention<TestController>
    {
        public void Apply(TestController item)
        {
            throw new System.NotImplementedException();
        }
    }

    public class AnotherTestControllerConvention : IFubuConvention<TestController>
    {
        public void Apply(TestController item)
        {
            throw new System.NotImplementedException();
        }
    }

    public class TestBehaviorConvention : IFubuConvention<TestBehavior>
    {
        public void Apply(TestBehavior item)
        {
            throw new System.NotImplementedException();
        }
    }

    public class AnotherTestBehaviorConvention : IFubuConvention<TestBehavior>
    {
        public void Apply(TestBehavior item)
        {
            throw new System.NotImplementedException();
        }
    }

}
