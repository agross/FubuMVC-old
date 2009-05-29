using System;
using System.Linq.Expressions;
using FubuMVC.Core.Controller;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Controller.Results;
using Microsoft.Practices.ServiceLocation;

namespace FubuMVC.Core.Behaviors
{
    public interface ISupportResultOverride
    {
        IInvocationResult ResultOverride { get; set; }
    }

    public static class ResultOverride
    {
        public static IInvocationResult IfAvailable<TOutput>(TOutput output)
        {
            var overrider = output as ISupportResultOverride;
            return overrider == null ? null : overrider.ResultOverride;
        }
    }

    public static class ResultOverrideExtensions
    {
        public static void RedirectTo<TController>(this ISupportResultOverride model, Expression<Func<TController, object>> actionExpression)
            where TController : class
        {
            var resolver = ServiceLocator.Current.GetInstance<IUrlResolver>();
            var url = resolver.UrlFor(actionExpression);
            model.RedirectTo(url);
        }

        public static void RedirectTo(this ISupportResultOverride model, string url)
        {
            model.ResultOverride = new RedirectResult(url);
        }
    }

}