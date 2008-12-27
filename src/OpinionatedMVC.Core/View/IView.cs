using System.Web.UI;

namespace OpinionatedMVC.Core.View
{
    public interface IView
    {
        void SetViewModel(object viewModel);
        object GetViewModel();
        string NamePrefix { get; set; }
    }

    public interface IViewWithModel<VIEWMODEL> : IView
    {
        VIEWMODEL ViewModel { get; }
    }

    public class OpinionatedViewControl<VIEWMODEL> : UserControl, IViewWithModel<VIEWMODEL>
    {
        public string NamePrefix { get; set; }
        public VIEWMODEL ViewModel { get; private set; }
     
        object IView.GetViewModel(){ return ViewModel; }
        void IView.SetViewModel(object viewModel){ ViewModel = (VIEWMODEL) viewModel; }
    }

    public class OpinionatedViewPage<VIEWMODEL> : Page, IViewWithModel<VIEWMODEL>
    {
        public string NamePrefix { get; set; }
        public VIEWMODEL ViewModel { get; private set; }

        object IView.GetViewModel(){ return ViewModel; }
        void IView.SetViewModel(object viewModel){ ViewModel = (VIEWMODEL) viewModel; }
    }
}