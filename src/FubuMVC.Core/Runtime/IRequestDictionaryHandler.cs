using System.Collections.Generic;
using System.Web.Routing;
using FubuMVC.Core.Routing;

namespace FubuMVC.Core.Runtime
{
    public interface IRequestDictionaryHandler
    {
        IDictionary<string, object> GetDictionary(RequestContext requestContext);
    }

    public class RequestDictionaryHandler : IRequestDictionaryHandler
    {
        public IDictionary<string, object> GetDictionary(RequestContext requestContext)
        {
            return new AggregateDictionary(requestContext.HttpContext.Request, requestContext.RouteData);
        }
    }
}