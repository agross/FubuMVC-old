using System.Web;

namespace FubuMVC.Core.Routing
{
    public interface IOutputWriter
    {
        void Write(string contentType, string renderedOutput);
        void RedirectToUrl(string url);
    }

    public class HttpResponseOutputWriter : IOutputWriter
    {
        public void Write(string contentType, string renderedOutput)
        {
            var response = HttpContext.Current.Response;
            response.ContentType = contentType;
            response.Write(renderedOutput);
        }

        public void RedirectToUrl(string url)
        {
            var response = HttpContext.Current.Response;
            response.Redirect(url, false);
        }
    }
}