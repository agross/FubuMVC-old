using System.Net.Mime;
using FubuMVC.Core.Routing;

namespace FubuMVC.Core.Results
{
    public class RenderHtmlFriendlyJsonResult<OUTPUT> : RenderJsonResult<OUTPUT>
    {
        public RenderHtmlFriendlyJsonResult(OUTPUT output) : base(output)
        {
        }

        protected override void writeJsonToOutput(IOutputWriter writer, string json)
        {
            writer.Write(MediaTypeNames.Text.Html, "<html><body><textarea>" + json + "</textarea></body></html>");
        }
    }
}