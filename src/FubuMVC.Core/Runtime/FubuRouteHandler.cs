using System;
using System.Web;
using System.Web.Routing;
using System.Web.SessionState;
using FubuMVC.Core.Config;

namespace FubuMVC.Core.Runtime
{
    public interface IFubuRouteHandler : IRouteHandler
    {
        UrlAction Action { get; }
    }

    public class FubuRouteHandler : IFubuRouteHandler
    {
        private readonly IActionInvoker _invoker;
        private readonly ICurrentAction _currentAction;
        private readonly IRequestDictionaryHandler _requestDictionaryHandler;

        public FubuRouteHandler(UrlAction action, IActionInvoker invoker, ICurrentAction currentAction, IRequestDictionaryHandler requestDictionaryHandler)
        {
            _invoker = invoker;
            _currentAction = currentAction;
            _requestDictionaryHandler = requestDictionaryHandler;
            Action = action;
        }

        public UrlAction Action { get; private set; }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            _currentAction.Current = Action;

            var dictionary = _requestDictionaryHandler.GetDictionary(requestContext);

            return new FubuHttpHandler(ctx => _invoker.Invoke(dictionary));
        }

        public class FubuHttpHandler : IHttpHandler, IRequiresSessionState
        {
            private readonly Action<HttpContext> _processRequestAction;

            public FubuHttpHandler(Action<HttpContext> processRequestAction)
            {
                _processRequestAction = processRequestAction;
            }

            public void ProcessRequest(HttpContext context)
            {
                _processRequestAction(context);
            }

            public bool IsReusable { get { return false; } }
        }
    }
}