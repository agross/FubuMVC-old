using System.Web.Routing;
using AltOxite.Core.Config;
using AltOxite.Core.Persistence;
using FubuMVC.Core.Controller.Config;
using Microsoft.Practices.ServiceLocation;
using FubuMVC.Container.StructureMap.Config;
using StructureMap;

namespace AltOxite.Web
{
    public class Bootstrapper : IBootstrapper
    {
        private static bool _hasStarted;

        public virtual void BootstrapStructureMap()
        {
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry(new FrameworkServicesRegistry());
                x.AddRegistry(new AltOxiteWebRegistry());
                x.AddRegistry(new ControllerConfig());
            });

            ObjectFactory.AssertConfigurationIsValid();
            
            initialize_routes();
            
            setup_service_locator();
        }

        private static void setup_service_locator()
        {
            ServiceLocator.SetLocatorProvider(() => new StructureMapServiceLocator());
        }

        private static void initialize_routes()
        {
            ObjectFactory.GetInstance<IRouteConfigurer>().LoadRoutes(RouteTable.Routes);
        }

        public static void Restart()
        {
            if (_hasStarted)
            {
                ObjectFactory.ResetDefaults();
            }
            else
            {
                Bootstrap();
                _hasStarted = true;
            }

        }

        public static void Bootstrap()
        {
            
            new Bootstrapper().BootstrapStructureMap();
        }
    }
}