using System.Net.Mime;
using Microsoft.Practices.ServiceLocation;
using FubuMVC.Core.Routing;
using FubuMVC.Core.View;

namespace FubuMVC.Core.Results
{
    public class RenderViewResult: IInvocationResult
    {
        private readonly string _viewToRender;

        public RenderViewResult(string viewToRender)
        {
            _viewToRender = viewToRender;
        }

        public const string HtmlContentType = MediaTypeNames.Text.Html;

        public void Execute(IServiceLocator locator)
        {
            var renderer = locator.GetInstance<IViewRenderer>();
            var writer = locator.GetInstance<IOutputWriter>();

            var content = renderer.RenderView(_viewToRender);
            writer.Write(HtmlContentType, content);
        }
    }
}