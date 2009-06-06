using System;
using System.Linq.Expressions;
using FubuMVC.Core.Controller;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Controller.Results;
using Microsoft.Practices.ServiceLocation;

namespace FubuMVC.Core.Behaviors
{
    [Obsolete("To override a result, consume the IResultOverride and use the OverrideResult property")]
    public interface ISupportResultOverride
    {
        [Obsolete("To override a result, consume the IResultOverride and use the OverrideResult property")]
        IInvocationResult ResultOverride { get; set; }
    }

    public static class ResultOverride
    {
        [Obsolete("To override a result, consume the IResultOverride and use the OverrideResult property")]
        public static IInvocationResult IfAvailable<TOutput>(TOutput output)
        {
            var overrider = output as ISupportResultOverride;
            return overrider == null ? null : overrider.ResultOverride;
        }
    }

    public static class ResultOverrideExtensions
    {
        [Obsolete("To override a result, consume the IResultOverride and use the OverrideResult property")]
        public static void RedirectTo<TController>(this ISupportResultOverride model, Expression<Func<TController, object>> actionExpression)
            where TController : class
        {
            var resolver = ServiceLocator.Current.GetInstance<IUrlResolver>();
            var url = resolver.UrlFor(actionExpression);
            model.RedirectTo(url);
        }

        [Obsolete("To override a result, consume the IResultOverride and use the OverrideResult property")]
        public static void RedirectTo(this ISupportResultOverride model, string url)
        {
            model.ResultOverride = new RedirectResult(url);
        }
    }

}