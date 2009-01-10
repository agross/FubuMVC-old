using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;

namespace FubuMVC.Core.Controller.Config
{
    public interface IRouteConfigurer
    {
        void LoadRoutes(RouteCollection routeCollection);
    }

    public class RouteConfigurer : IRouteConfigurer
    {
        private readonly IList<Route> _registeredRoutes = new List<Route>();
        private readonly FubuConventions _conventions;
        private readonly FubuConfiguration _config;
        private readonly HashSet<Type> _alreadyVisitedControllerTypes = new HashSet<Type>();

        public RouteConfigurer(FubuConfiguration configuration, FubuConventions conventions)
        {
            _config = configuration;
            _conventions = conventions;

            _config.GetControllerActionConfigs().Each(ConfigureAction);
        }
        
        public Route AppDefaultRoute { get; private set; }

        public void ConfigureAction(ControllerActionConfig config)
        {
            set_this_as_app_default_if_necessary(config);

            register_Routes(config);
        }

        private void register_Routes(ControllerActionConfig config)
        {
            if (! _alreadyVisitedControllerTypes.Contains(config.ControllerType))
            {
                _registeredRoutes.Add(CreateRoute(config, _config.GetDefaultUrlFor(config.ControllerType)));
                _alreadyVisitedControllerTypes.Add(config.ControllerType);
            }

            _registeredRoutes.Add(CreateRoute(config, config.PrimaryUrl));

            config.GetOtherUrls().Each(url => _registeredRoutes.Add(CreateRoute(config, url)));
        }

        private void set_this_as_app_default_if_necessary(ControllerActionConfig config)
        {
            if (AppDefaultRoute != null && !_conventions.IsAppDefaultUrl(config)) return;
            
            _registeredRoutes.Remove(AppDefaultRoute);
            AppDefaultRoute = CreateRoute(config, "");
            _registeredRoutes.Add(AppDefaultRoute);
        }

        public Route CreateRoute(ControllerActionConfig config, string urlFormat)
        {
            try
            {
                return new Route(urlFormat, new ActionRouteHandler(config));
            }
            catch (ArgumentException aex)
            {
                throw new ArgumentException("Could not create route with URL format '{0}'. See inner exception for details.".ToFormat(urlFormat), aex);
            }
            
        }

        public IEnumerable<Route> GetRegisteredRoutes()
        {
            return _registeredRoutes.AsEnumerable();
        }

        public void LoadRoutes(RouteCollection routeCollection)
        {
            _registeredRoutes.Each(r => routeCollection.Add(r));
        }
    }
}
