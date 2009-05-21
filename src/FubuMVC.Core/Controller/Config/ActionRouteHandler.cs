using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using System.Web.SessionState;
using FubuMVC.Core.Controller.Invokers;
using Microsoft.Practices.ServiceLocation;
using FubuMVC.Core.Routing;

namespace FubuMVC.Core.Controller.Config
{
    public interface IActionRoute : IRouteHandler
    {
        ControllerActionConfig Config { get; }
    }

    public class ActionRouteHandler : IActionRoute
    {
        public ActionRouteHandler(ControllerActionConfig config)
        {
            GetRequestDataFromContext = context => new AggregateDictionary(context.HttpContext.Request, context.RouteData);
            Config = config;
        }

        public ControllerActionConfig Config { get; private set; }

        public Func<RequestContext, IDictionary<string, object>> GetRequestDataFromContext { get; set; }
       
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            var requestData = GetRequestDataFromContext(requestContext);

            return new ActionHttpHandler(Config, requestData);
        }
    }

    public class ActionHttpHandler : IHttpHandler, IRequiresSessionState
    {
        private readonly IDictionary<string, object> _requestData;
        private readonly ControllerActionConfig _config;

        public ActionHttpHandler(ControllerActionConfig config, IDictionary<string, object> requestData)
        {
            _config = config;
            _requestData = requestData;
        }

        public virtual void HandleRequest()
        {
            var locator = ServiceLocator.Current;

            //TODO: This probably doesn't belong here. Need to rethink this part
            var localization = locator.GetInstance<ILocalization>();
            localization.Configure(_requestData);
            
            var configContext = locator.GetInstance<IControllerConfigContext>();
            configContext.CurrentConfig = _config;

            var invoker = locator.GetInstance<IControllerActionInvoker>(_config.UniqueID);
            invoker.Invoke(_requestData);
        }

        #region IHttpHandler stuff
        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            HandleRequest();
        }

        bool IHttpHandler.IsReusable
        {
            get { return false; }
        }
        #endregion
    }
}
