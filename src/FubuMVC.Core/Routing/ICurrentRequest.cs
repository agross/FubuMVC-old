using System;
using System.Web;

namespace FubuMVC.Core.Routing
{
    public interface ICurrentRequest
    {
        Uri GetUrl();
    }

    public class CurrentRequest : ICurrentRequest
    {
        private readonly HttpContext _httpContext;

        public CurrentRequest()
        {
            _httpContext = HttpContext.Current;
        }

        public Uri GetUrl()
        {
            return _httpContext.Request.Url;
        }
    }
}