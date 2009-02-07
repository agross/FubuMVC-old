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

        string PrimaryApplicationUrl();
        string PageNotFoundUrl();
    }

    public class UrlResolver : IUrlResolver
    {
        private readonly FubuConfiguration _config;
        private readonly FubuConventions _conventions;

        public UrlResolver(FubuConfiguration config, FubuConventions conventions)
        {
            _config = config;
            _conventions = conventions;
        }

        public string UrlFor<CONTROLLER>()
            where CONTROLLER : class
        {
            return ToFullyResolvedPath(_config.GetDefaultUrlFor<CONTROLLER>());
        }

        public string UrlFor<CONTROLLER>(Expression<Func<CONTROLLER, object>> actionExpression)
            where CONTROLLER : class
        {
            return ToFullyResolvedPath(_config.GetDefaultUrlFor(actionExpression));
        }

        public string PrimaryApplicationUrl()
        {
            return ToFullyResolvedPath(_conventions.PrimaryApplicationUrl).ToFullUrl();
        }

        public string PageNotFoundUrl()
        {
            return ToFullyResolvedPath(_conventions.PageNotFoundUrl).ToFullUrl();
        }

        public static string ToFullyResolvedPath(string actionUrl)
        {
            return ("~/" + actionUrl).ToFullUrl();
        }
    }
}