using FubuMVC.Core.Routing;
using FubuMVC.Core.Util;
using Microsoft.Practices.ServiceLocation;

namespace FubuMVC.Core.Controller.Results
{
    public class RenderRssResult : IInvocationResult
    {
        private readonly Feed _feed;
        public static readonly string RSS_CONTENT_TYPE = "text/xml;charset=utf-8";

        public RenderRssResult(Feed feed)
        {
            _feed = feed;
        }

        public void Execute(IServiceLocator locator)
        {
            var writer = locator.GetInstance<IOutputWriter>();
            var rss = new RssUtil().ToRss(_feed);
            writeRssToOutput(writer, rss);
        }

        protected virtual void writeRssToOutput(IOutputWriter writer, string rss)
        {
            writer.Write(RSS_CONTENT_TYPE, rss);
        }
    }
}