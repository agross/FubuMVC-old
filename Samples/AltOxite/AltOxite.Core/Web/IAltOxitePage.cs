using FubuMVC.Core.View;

namespace AltOxite.Core.Web
{
    public interface IAltOxitePage : IFubuViewWithModel
    {
        object Model{ get; }
    }
}