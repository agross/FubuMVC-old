using FubuMVC.Core.View;

namespace VsTemplate.Core.Web
{
    public interface IFubuMvcPage : IFubuViewWithModel
    {
        object Model{ get; }
    }
}