using System;
using System.Web;

namespace FubuMVC.Core.Html
{
    public static class UrlContext
    {
        static UrlContext()
        {
            Reset();
        }

        public static void Reset()
        {
            if (HttpRuntime.AppDomainAppVirtualPath != null)
            {
                Live();
                return;
            }

            Stub("");
        }

        public static void Stub()
        {
            Stub("");    
        }

        public static void Stub(string usingFakeUrl)
        {
            Combine = (basePath, subPath) => "{0}/{1}".ToFormat(basePath.TrimEnd('/'), subPath.TrimStart('/'));
            ToAbsolute = path => Combine(usingFakeUrl, path.Replace("~", ""));
            ToFull = path => Combine(usingFakeUrl, path.Replace("~", ""));
            ToPhysicalPath = virtPath => virtPath.Replace("~", "").Replace("/", "\\");
        }

        public static void Live()
        {
            Combine = VirtualPathUtility.Combine;
            ToAbsolute = path =>
            {
                var result = path.Replace("~", VirtualPathUtility.ToAbsolute("~"));
                return result.StartsWith("//") ? result.Substring(1) : result;
            };
            ToFull = path =>
            {
                var baseUri = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);
                return new Uri(baseUri, ToAbsolute(path)).ToString();
            };
            ToPhysicalPath = HttpContext.Current.Server.MapPath;
        }

        public static Func<string, string, string> Combine{ get; private set;}
        public static Func<string, string> ToAbsolute{ get; private set;}
        public static Func<string, string> ToFull{ get; private set;}
        public static Func<string, string> ToPhysicalPath { get; private set; }

        public static string GetUrl(string url)
        {
            return ToAbsolute(Combine("~/", url));
        }

        public static string GetFullUrl(string path)
        {
            return ToFull(path);
        }
    }
}