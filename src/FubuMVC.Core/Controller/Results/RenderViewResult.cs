using System.Net.Mime;
using Microsoft.Practices.ServiceLocation;
using FubuMVC.Core.Routing;
using FubuMVC.Core.View;

namespace FubuMVC.Core.Controller.Results
{
    public class RenderViewResult<OUTPUT>: IInvocationResult
            where OUTPUT : class
    {
        public const string HtmlContentType = MediaTypeNames.Text.Html;
        private readonly OUTPUT _output;

        public RenderViewResult(OUTPUT output)
        {
            _output = output;
        }

        public void Execute(IServiceLocator locator)
        {
            var renderer = locator.GetInstance<IViewRenderer>();
            var writer = locator.GetInstance<IOutputWriter>();

            var content = renderer.RenderView(_output);
            writer.Write(HtmlContentType, content);
        }
    }
}
