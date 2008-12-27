namespace FubuMVC.Core.View
{
    public interface IFubuView
    {
    }

    public interface IFubuViewWithModel : IFubuView
    {
        void SetModel(object model);
    }

    public interface IFubuView<VIEWMODEL> : IFubuViewWithModel
        where VIEWMODEL : class
    {
        VIEWMODEL Model { get; }
    }
}