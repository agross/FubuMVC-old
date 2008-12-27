using System;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Compilation;
using System.Web.UI;
using OpinionatedMVC.Core.View;

namespace OpinionatedMVC.Core.Html
{
    public interface IControlBuilder
    {
        object LoadControlFromVirtualPath(string virtualPath);
        void ExecuteControl(IHttpHandler handler, TextWriter writer);
    }

    public class AspNetControlBuilder : IControlBuilder
    {
        public object LoadControlFromVirtualPath(string virtualPath)
        {
            Type controlType = BuildManager.GetCompiledType(virtualPath);
            return Activator.CreateInstance(controlType);
        }

        public void ExecuteControl(IHttpHandler handler, TextWriter writer)
        {
            HttpContext.Current.Server.Execute(handler, writer, true);
        }
    }

    public interface IPartialRenderer
    {
        IView CreateControl<T>() where T : IView;
        IView CreateControl(Type controlType);

        string Render(IView view, object viewModel, string prefix);
        void Render(IView view, object viewModel, string prefix, TextWriter writer);

        string Render(Type controlType, object viewModel, string prefix);
        void Render(Type controlType, object viewModel, string prefix, TextWriter writer);
    }

    public class PartialRenderer : IPartialRenderer
    {
        private readonly IControlBuilder _builder;

        public PartialRenderer(IControlBuilder builder)
        {
            _builder = builder;
        }

        public IView CreateControl<VIEW>() where VIEW : IView
        {
            return CreateControl(typeof (VIEW));
        }

        public IView CreateControl(Type controlType)
        {
            if (!typeof(IView).IsAssignableFrom(controlType) || !typeof(Control).IsAssignableFrom(controlType))
            {
                throw new InvalidOperationException(String.Format(
                                                        "PartialRenderer cannot render type '{0}'. It can only render System.Web.UI.Control objects which implement the IView interface.",
                                                        (controlType == null) ? "(null)" : controlType.Name));
            }

            string virtualPath = controlType.ToVirtualPath();
            var control = _builder.LoadControlFromVirtualPath(virtualPath);
            return control as IView;
        }

        #region IPartialRenderer Members

        public string Render(IView view, object viewModel, string prefix)
        {
            var writer = new StringWriter(CultureInfo.CurrentCulture);
            Render(view, viewModel, prefix, writer);
            return writer.GetStringBuilder().ToString();
        }

        public void Render(IView view, object viewModel, string prefix, TextWriter writer)
        {
            var page = new Page();
            page.Controls.Add(view as Control);

            view.SetViewModel(viewModel);
            view.NamePrefix = prefix;

            _builder.ExecuteControl(page, writer);

            writer.Flush();
        }

        public string Render(Type controlType, object viewModel, string prefix)
        {
            var _viewToRender = CreateControl(controlType);
            return Render(_viewToRender, viewModel, prefix);
        }

        public void Render(Type controlType, object viewModel, string prefix, TextWriter writer)
        {
            var view = CreateControl(controlType);

            Render(view, viewModel, prefix, writer);
        }

        #endregion
    }
}