using FubuMVC.Core.View;

namespace AltOxite.Core.Web
{
    public interface IAltOxitePage : IFubuViewWithModel
    {
        ViewModel Model{ get; }
    }
}