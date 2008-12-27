using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
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
        private readonly Type _handlerType;

        public ActionRouteHandler(ControllerActionConfig config)
        {
            GetRequestDataFromContext = context => new AggregateDictionary(context.HttpContext.Request, context.RouteData);

            Config = config;
            _handlerType =  typeof (ActionHttpHandler<,,>).MakeGenericType(
                Config.ControllerType,
                Config.InputType,
                Config.OutputType);
        }

        public ControllerActionConfig Config { get; private set; }

        public Func<RequestContext, IDictionary<string, object>> GetRequestDataFromContext { get; set; }
       
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            var requestData = GetRequestDataFromContext(requestContext);

            return (IHttpHandler)Activator.CreateInstance(_handlerType, Config, requestData);
        }
    }

    public class ActionHttpHandler<CONTROLLER, INPUT, OUTPUT> : IHttpHandler where CONTROLLER : class where INPUT : class, new() where OUTPUT : class
    {
        private readonly Func<CONTROLLER, INPUT, OUTPUT> _actionFunc;
        private readonly IDictionary<string, object> _requestData;
        private readonly ControllerActionConfig _config;

        public ActionHttpHandler(ControllerActionConfig config, IDictionary<string, object> requestData)
        {
            _config = config;
            _requestData = requestData;
            _actionFunc = config.GetActionFunc<CONTROLLER, INPUT, OUTPUT>();
        }

        public virtual void HandleRequest()
        {
            var locator = ServiceLocator.Current;
            
            var configContext = locator.GetInstance<IControllerConfigContext>();
            configContext.CurrentConfig = _config;

            var invoker = locator.GetInstance<IControllerActionInvoker<CONTROLLER, INPUT, OUTPUT>>(_config.UniqueID);
            invoker.Invoke(_actionFunc, _requestData);
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
