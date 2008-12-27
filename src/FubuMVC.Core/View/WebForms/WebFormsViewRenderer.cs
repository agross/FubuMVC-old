using System;
using FubuMVC.Core.Controller.Config;

namespace FubuMVC.Core.View.WebForms
{
    public class WebFormsViewRenderer : IWebFormsViewRenderer
    {
        private readonly IWebFormsControlBuilder _webFormsBuilder;
        private FubuConventions _conventions;
        private IControllerConfigContext _context;

        public WebFormsViewRenderer(FubuConventions conventions, IControllerConfigContext context, IWebFormsControlBuilder webFormsBuilder)
        {
            _webFormsBuilder = webFormsBuilder;
            _conventions = conventions;
            _context = context;
        }

        public string RenderView<OUTPUT>(OUTPUT output)
            where OUTPUT : class
        {
            return RenderView<IFubuViewWithModel>(output, null);
        }

        public string RenderView<VIEWTYPE>(object model, Action<VIEWTYPE> viewSetupAction)
            where VIEWTYPE : class, IFubuViewWithModel
        {
            var virtualPath = _conventions.DefaultPathToViewForAction(_context.CurrentConfig);

            return RenderView(virtualPath, model, viewSetupAction);
        }

        public string RenderView<VIEWTYPE>(string virtualPath, object model, Action<VIEWTYPE> viewSetupAction)
            where VIEWTYPE : class, IFubuViewWithModel
        {
            var control = _webFormsBuilder.LoadControlFromVirtualPath(virtualPath, typeof(IFubuViewWithModel));

            if (control != null)
            {
                ((IFubuViewWithModel)control).SetModel(model);

                if (viewSetupAction != null)
                {
                    viewSetupAction(control as VIEWTYPE);
                }
            }

            return _webFormsBuilder.ExecuteControl(control);
        }
    }
}
