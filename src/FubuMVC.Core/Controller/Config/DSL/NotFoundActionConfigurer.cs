using System;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Controller.Invokers;
using FubuMVC.Core.Util;

namespace FubuMVC.Core.Controller.Config.DSL
{
    public class NotFoundActionConfigurer
    {
        public const string ALL_HANDLER_URL = "{*url}";

        private readonly FubuConfiguration _config;

        public NotFoundActionConfigurer(FubuConfiguration configuration)
        {
            _config = configuration;
        }

        public void Configure()
        {
            var notFoundAction =
                new ControllerActionConfig(typeof(PureBehaviorActionInvoker<object, object>))
                {
                    PrimaryUrl = ALL_HANDLER_URL,
                    ActionName = "NotFoundHandler",
                    ControllerType = typeof(RedirectToNotFoundUrl),
                    ActionMethod = ReflectionHelper.GetMethod<RedirectToNotFoundUrl>(i => i.Invoke<object, object>(i, null)),
                    ActionDelegate = new Action<object>(i => { })
                };
            _config.AddControllerActionConfig(notFoundAction);
            notFoundAction.RemoveAllOtherUrls();
            notFoundAction.RemoveAllBehaviors();
            notFoundAction.AddBehavior<execute_the_result>();
            notFoundAction.AddBehavior<RedirectToNotFoundUrl>();
        }
    }
}