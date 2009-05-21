using System;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Controller.Invokers;
using FubuMVC.Core.Util;

namespace FubuMVC.Core.Controller.Config.DSL
{
    public class DebugOutputActionConfigurer
    {
        public const string DEBUG_URL = "__debug";

        private readonly FubuConfiguration _config;
        private readonly FubuConventions _conventions;
        

        public DebugOutputActionConfigurer(FubuConventions conventions, FubuConfiguration configuration)
        {
            _conventions = conventions;
            _config = configuration;
        }

        public void Configure()
        {
            if (!_conventions.DebugMode()) return;

            var debugAction =
                new ControllerActionConfig(typeof(PureBehaviorActionInvoker<object, object>))
                {
                    PrimaryUrl = DEBUG_URL,
                    ActionName = DEBUG_URL,
                    ControllerType = typeof(OutputDebugInformation),
                    ActionMethod = ReflectionHelper.GetMethod<OutputDebugInformation>(i => i.Invoke<object, object>(i, null)),
                    ActionDelegate = new Action<object>(i => { })
                };
            _config.AddControllerActionConfig(debugAction);
            debugAction.RemoveAllBehaviors();
            debugAction.AddBehavior<execute_the_result>();
            debugAction.AddBehavior<OutputDebugInformation>();
        }
    }
}