using FubuMVC.Tests.Behaviors;

namespace FubuMVC.Tests.Controller.Config.DSL
{
    public class MethodDummy
    {
        private string PrivateMethod(string blah) { return ""; }
        public string GenericMethod<T>(string blah) { return ""; }
        public void VoidMethod(TestViewModel blah) { }
        public void VoidInvalidMethod(string blah) { }
        public string NoParamMethod() { return ""; }
        public string ManyParamMethod(string blah, string foo) { return ""; }
        public string ValueParamMethod(int blah) { return ""; }
        public int ValueReturnMethod(string blah) { return 0; }
        public string OMIOMOMethodBadInput(string blah) { return "";  }
        public string GoodMethod(TestViewModel blah) { return ""; }
    }
}