using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using FubuMVC.Core.Runtime;

namespace FubuMVC.Core.Config
{
    public interface IRouteConfigurer
    {
        void LoadRoutes(RouteCollection routeCollection);
    }

    public class RouteConfigurer : IRouteConfigurer
    {
        private readonly IFubuRouteHandler[] _routeHandlers;
        private readonly IList<Route> _registeredRoutes = new List<Route>();

        public RouteConfigurer(IFubuRouteHandler[] routeHandlers)
        {
            _routeHandlers = routeHandlers;
        }

        public void ConfigureRoute(IFubuRouteHandler config)
        {
            config.Action.AllUrlStubs.Each(url => _registeredRoutes.Add(CreateRoute(url, config)));
        }

        public Route CreateRoute(string urlStub, IFubuRouteHandler handler)
        {
            try
            {
                return new Route(urlStub, handler);
            }
            catch (ArgumentException aex)
            {
                throw new ArgumentException("Could not create route with URL format '{0}'. See inner exception for details.".ToFormat(urlStub), aex);
            }
            
        }

        public IEnumerable<Route> GetRegisteredRoutes()
        {
            return _registeredRoutes.AsEnumerable();
        }

        public void Configure()
        {
            _routeHandlers.Each(ConfigureRoute);
        }

        public void LoadRoutes(RouteCollection routeCollection)
        {
            if( _registeredRoutes.Count == 0 ) Configure();

            _registeredRoutes.Each(routeCollection.Add);
        }
    }
}