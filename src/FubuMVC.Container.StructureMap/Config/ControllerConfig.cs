using System;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Controller.Config.DSL;
using StructureMap.Configuration.DSL;

namespace FubuMVC.Container.StructureMap.Config
{
    public class ControllerConfig : Registry
    {
        public static Action<ControllerActionDSL> Configure = x =>
        {
            throw new NotImplementedException(@"
                Please configure controllers and actions by supplying a 
                ControllerConfig.Configure delegate and then calling 
                Bootstrapper.Restart() to configure and initialize this 
                application.");
        };

        public ControllerConfig()
        {
            var conventions = new FubuConventions();
            var configuration = new FubuConfiguration(conventions);

            var dsl = new ControllerActionDSL(configuration, conventions);

            Configure(dsl);

            var configurer = new StructureMapConfigurer(conventions, configuration);
            configurer.ConfigureRegistry(this);
        }
    }
}