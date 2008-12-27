using System;
using System.Linq.Expressions;

namespace FubuMVC.Core.Controller.Config
{
    public interface IUrlResolver
    {
        string UrlFor<CONTROLLER>()
            where CONTROLLER : class;

        string UrlFor<CONTROLLER>(Expression<Func<CONTROLLER, object>> actionExpression)
            where CONTROLLER : class;
    }

    public class UrlResolver : IUrlResolver
    {
        private readonly FubuConfiguration _config;

        public UrlResolver(FubuConfiguration config)
        {
            _config = config;
        }

        public string UrlFor<CONTROLLER>()
            where CONTROLLER : class
        {
            return toFullyResolvedPath(_config.GetDefaultUrlFor<CONTROLLER>());
        }

        public string UrlFor<CONTROLLER>(Expression<Func<CONTROLLER, object>> actionExpression)
            where CONTROLLER : class
        {
            return toFullyResolvedPath(_config.GetDefaultUrlFor(actionExpression));
        }

        public static string toFullyResolvedPath(string actionUrl)
        {
            return ("~/" + actionUrl).ToFullUrl();
        }
    }
}