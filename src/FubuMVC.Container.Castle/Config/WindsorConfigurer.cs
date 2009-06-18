using System;
using System.Linq;

using Castle.MicroKernel.Registration;
using Castle.Windsor;

using FubuMVC.Container.Castle.ForIoC;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Controller.Invokers;
using FubuMVC.Core.Conventions;
using FubuMVC.Core.Util;

namespace FubuMVC.Container.Castle.Config
{
	public class WindsorConfigurer
	{
		static readonly string INSIDE_PROP_NAME =
			ReflectionHelper.GetProperty<IControllerActionBehavior>(b => b.InsideBehavior).Name;

		readonly FubuConfiguration _configuration;
		readonly FubuConventions _conventions;

		public WindsorConfigurer(FubuConventions conventions, FubuConfiguration configuration)
		{
			_conventions = conventions;
			_configuration = configuration;
		}

		public void ConfigureContainer(IWindsorContainer container)
		{
			container.AddFacility<ArrayDependencyFacility>();

			container.Register(Component.For<FubuConventions>().Instance(_conventions));
			container.Register(Component.For<FubuConfiguration>().Instance(_configuration));

			_conventions.GetCustomConventionTargetTypes().Each(target =>
				{
					var interfaceType = typeof(IFubuConvention<>).MakeGenericType(target);
					var conventions = _conventions.GetCustomConventionTypesFor(target);

					conventions.Each(conventionType => container.Register(Component.For(interfaceType)
					                                                      	.ImplementedBy(conventionType)
					                                                      	.LifeStyle.Transient));
				});

			ComponentRegistration<object> defaultBehavior = Component.For(typeof(DefaultBehavior))
				.ImplementedBy<DefaultBehavior>()
				.LifeStyle.Transient;
			container.Register(defaultBehavior);

			_configuration.GetControllerActionConfigs().Each(e =>
				{
					container.Register(Component.For(e.ControllerType)
					                   	.ImplementedBy(e.ControllerType)
					                   	.LifeStyle.Transient
					                   	.Unless(Component.ServiceAlreadyRegistered));

					var configurer = GetConfigurer(e);

					var behavior = defaultBehavior;
					e.GetBehaviors().Reverse().Each(t =>
						{
							behavior = Component.For(t)
								.LifeStyle.Transient
								.Named(Guid.NewGuid().ToString())
								.ServiceOverrides(ServiceOverride.ForKey(INSIDE_PROP_NAME).Eq(behavior.Name));
							container.Register(behavior);
						});

					configurer.Config(container, behavior);
				});
		}

		static IInvokerInstanceConfigurer GetConfigurer(ControllerActionConfig config)
		{
			var configurerType = typeof(InvokerInstanceConfigurer<>).MakeGenericType(config.InvokerType);

			return (IInvokerInstanceConfigurer) Activator.CreateInstance(configurerType, config);
		}

		#region Nested type: IInvokerInstanceConfigurer
		interface IInvokerInstanceConfigurer
		{
			void Config(IWindsorContainer container, ComponentRegistration<object> behaviorChain);
		}
		#endregion

		#region Nested type: InvokerInstanceConfigurer
		class InvokerInstanceConfigurer<TInvokerType> : IInvokerInstanceConfigurer
			where TInvokerType : IControllerActionInvoker
		{
			readonly ControllerActionConfig _config;

			public InvokerInstanceConfigurer(ControllerActionConfig config)
			{
				_config = config;
			}

			#region IInvokerInstanceConfigurer Members
			public void Config(IWindsorContainer container, ComponentRegistration<object> behaviorChain)
			{
				container.Register(Component.For<IControllerActionInvoker>()
				                   	.ImplementedBy<TInvokerType>()
				                   	.LifeStyle.Transient
				                   	.Named(_config.UniqueID)
									.ServiceOverrides(ServiceOverride.ForKey("behavior").Eq(behaviorChain.Name)));
			}
			#endregion
		}
		#endregion
	}
}