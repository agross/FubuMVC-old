using System;
using System.IO;
using System.Web;
using System.Web.Compilation;
using System.Web.UI;

namespace FubuMVC.Core.View.WebForms
{
    public interface IWebFormsControlBuilder
    {
        Control LoadControlFromVirtualPath(string virtualPath, Type mustImplementInterfaceType);
        string ExecuteControl(Control control);
    }

    // THAR BE DRAGONS!
    // Untestable ASP.NET code to follow....

    public class WebFormsControlBuilder : IWebFormsControlBuilder
    {
        public virtual Control LoadControlFromVirtualPath(string virtualPath, Type mustImplementInterfaceType)
        {
            return (Control) BuildManager.CreateInstanceFromVirtualPath(virtualPath, mustImplementInterfaceType);
        }

        public string ExecuteControl(Control control)
        {
            var handler = GetHandler(control);
            return ExecuteHandler(handler);
        }

        public virtual IHttpHandler GetHandler(Control control)
        {
            var handler = control as IHttpHandler;

            if (handler == null)
            {
                var holderPage = new Page();
                holderPage.Controls.Add(control);
                handler = holderPage;
            }

            return handler;
        }

        public virtual string ExecuteHandler(IHttpHandler handler)
        {
            var writer = new StringWriter();
            HttpContext.Current.Server.Execute(handler, writer, true);
            writer.Flush();
            return writer.GetStringBuilder().ToString();
        }
    }
}