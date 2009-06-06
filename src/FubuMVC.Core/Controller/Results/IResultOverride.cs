using System;
using System.Linq.Expressions;
using FubuMVC.Core.Controller.Config;
using Microsoft.Practices.ServiceLocation;

namespace FubuMVC.Core.Controller.Results
{
    public interface IResultOverride
    {
        IInvocationResult OverrideResult { get; set; }
        IInvocationResult OverrideIfNotAlreadyOverriden(IInvocationResult result);
    }

    public class CurrentRequestResultOverride : IResultOverride
    {
        public IInvocationResult OverrideResult { get; set; }
        
        public IInvocationResult OverrideIfNotAlreadyOverriden(IInvocationResult result)
        {
            return OverrideResult ?? (OverrideResult = result);
        }
    }

    public static class ResultOverrideExtensions
    {
        public static void RedirectTo<TController>(this IResultOverride overrider, Expression<Func<TController, object>> actionExpression)
            where TController : class
        {
            var resolver = ServiceLocator.Current.GetInstance<IUrlResolver>();
            var url = resolver.UrlFor(actionExpression);
            overrider.RedirectTo(url);
        }

        public static void RedirectTo(this IResultOverride overrider, string url)
        {
            overrider.OverrideResult = new RedirectResult(url);
        }
    }
}