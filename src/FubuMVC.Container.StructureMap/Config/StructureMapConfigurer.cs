using System;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Controller;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Util;
using StructureMap.Configuration.DSL;
using StructureMap.Pipeline;

namespace FubuMVC.Container.StructureMap.Config
{
    public class StructureMapConfigurer
    {
        private static readonly string INSIDE_PROP_NAME = ReflectionHelper.GetProperty<IControllerActionBehavior>(b => b.InsideBehavior).Name;

        private readonly FubuConfiguration _configuration;
        private readonly FubuConventions _conventions;

        public StructureMapConfigurer(FubuConventions conventions, FubuConfiguration configuration)
        {
            _conventions = conventions;
            _configuration = configuration;
        }

        public void ConfigureRegistry(Registry registry)
        {
            registry.ForRequestedType<FubuConventions>().TheDefault.IsThis(_conventions);
            registry.ForRequestedType<FubuConfiguration>().TheDefault.IsThis(_configuration);

            _configuration.GetControllerActionConfigs().Each(e =>
            {
                registry.ForRequestedType(e.ControllerType).TheDefaultIsConcreteType(e.ControllerType);

                var configurer = GetConfigurer(e);

                var behaviorInstance = new ConfiguredInstance(typeof(DefaultBehavior));

                e.GetBehaviors().Each(t =>
                {
                    behaviorInstance = new ConfiguredInstance(t)
                        .SetterDependency<IControllerActionBehavior>(INSIDE_PROP_NAME)
                        .Is(behaviorInstance);
                });

                configurer.Config(registry, behaviorInstance);
            });
        }

        public interface IInvokerInstanceConfigurer
        {
            void Config(Registry registry, Instance behaviorInstance);
        }

        public class InvokerInstanceConfigurer<CONTROLLER, INPUT, OUTPUT> : IInvokerInstanceConfigurer
            where CONTROLLER : class
            where INPUT : class, new()
            where OUTPUT : class
        {
            private readonly ControllerActionConfig _config;

            public InvokerInstanceConfigurer(ControllerActionConfig config)
            {
                _config = config;
            }
            
            public void Config(Registry registry, Instance behaviorInstance)
            {
                registry.ForRequestedType<IControllerActionInvoker<CONTROLLER, INPUT, OUTPUT>>()
                    .TheDefault.Is.OfConcreteType<ThunderdomeActionInvoker<CONTROLLER, INPUT, OUTPUT>>()
                    .WithName(_config.UniqueID)
                    .CtorDependency<IControllerActionBehavior>().Is(behaviorInstance);
            }
        }

        public static IInvokerInstanceConfigurer GetConfigurer(ControllerActionConfig config)
        {
            var configurerType = typeof(InvokerInstanceConfigurer<,,>).MakeGenericType(config.ControllerType, config.InputType, config.OutputType);

            return (IInvokerInstanceConfigurer) Activator.CreateInstance(configurerType, config);
        }
    }
}