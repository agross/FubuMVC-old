using Microsoft.Practices.ServiceLocation;
using FubuMVC.Core.Routing;
using FubuMVC.Core.Util;

namespace FubuMVC.Core.Controller.Results
{
    public class RenderJsonResult<OUTPUT> : IInvocationResult
    {
        private readonly OUTPUT _output;
        public static readonly string JSON_CONTENT_TYPE = "application/json";

        public RenderJsonResult(OUTPUT output)
        {
            _output = output;
        }

        public void Execute(IServiceLocator locator)
        {
            var writer = locator.GetInstance<IOutputWriter>();
            var json = JsonUtil.ToJson(_output);
            writeJsonToOutput(writer, json);
        }

        protected virtual void writeJsonToOutput(IOutputWriter writer, string json)
        {
            writer.Write(JSON_CONTENT_TYPE, json);
        }
    }
}
