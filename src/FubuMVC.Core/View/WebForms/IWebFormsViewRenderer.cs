using System;

namespace FubuMVC.Core.View.WebForms
{
    public interface IWebFormsViewRenderer : IViewRenderer
    {
        string RenderView<VIEWTYPE>(object model, Action<VIEWTYPE> viewSetupAction)
            where VIEWTYPE : class, IFubuViewWithModel;

        string RenderView<VIEWTYPE>(string virtualPath, object model, Action<VIEWTYPE> viewSetupAction)
            where VIEWTYPE : class, IFubuViewWithModel;
    }
}
