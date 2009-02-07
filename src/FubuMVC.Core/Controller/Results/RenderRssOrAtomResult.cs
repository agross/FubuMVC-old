using System;
using System.Text;
using System.Xml;
using FubuMVC.Core.Routing;
using Microsoft.Practices.ServiceLocation;

namespace FubuMVC.Core.Controller.Results
{
    public class RenderRssOrAtomResult : IInvocationResult
    {
        private readonly Action<XmlWriter> _saveAs;
        private readonly string _contentType;
        public static readonly string RSS_CONTENT_TYPE = "application/rss+xml";
        public static readonly string ATOM_CONTENT_TYPE = "application/atom+xml";

        public RenderRssOrAtomResult(Action<XmlWriter> saveAs, string contentType)
        {
            _saveAs = saveAs;
            _contentType = contentType;
        }

        public void Execute(IServiceLocator locator)
        {
            var writer = locator.GetInstance<IOutputWriter>();

            StringBuilder stringBuilder = new StringBuilder();
            using (XmlWriter feedWriter = XmlWriter.Create(stringBuilder))
            {
                if (feedWriter != null)
                {
                    _saveAs(feedWriter);
                }
            }
            var rss = stringBuilder.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", ""); // TODO: Make this nicer
            writeRssToOutput(writer, rss);
        }

        protected virtual void writeRssToOutput(IOutputWriter writer, string rss)
        {
            writer.Write(_contentType, rss);
        }
    }
}