using System;

using Castle.MicroKernel;
using Castle.Windsor;

using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Controller.Config.DSL;

namespace FubuMVC.Container.Castle.Config
{
	public class ControllerConfig:IWindsorInstaller
	{
		public static Action<ControllerActionDSL> Configure =
			x =>
				{
					throw new NotImplementedException(
						@"
                Please configure controllers and actions by supplying a 
                ControllerConfig.Configure delegate and then calling 
                Bootstrapper.Restart() to configure and initialize this 
                application.");
				};

		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			var conventions = new FubuConventions();
			var configuration = new FubuConfiguration(conventions);

			var dsl = new ControllerActionDSL(configuration, conventions);

			Configure(dsl);

			//TODO: one day, these things will be conventionally discovered
			new DebugOutputActionConfigurer(conventions, configuration).Configure();
			new NotFoundActionConfigurer(configuration).Configure();

			var configurer = new WindsorConfigurer(conventions, configuration);
			configurer.ConfigureContainer(container);
		}
	}
}