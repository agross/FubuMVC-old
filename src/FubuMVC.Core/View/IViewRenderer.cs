namespace FubuMVC.Core.View
{
    public interface IViewRenderer
    {
        string RenderView<OUTPUT>(OUTPUT output)
            where OUTPUT : class;
    }
}
