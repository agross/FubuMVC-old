using System.Web.UI;
using FubuMVC.Core.View;

namespace AltOxite.Core.Web.WebForms
{
    public class AltOxiteUserControl<MODEL> : UserControl, IAltOxitePage, IFubuView<MODEL> 
        where MODEL : ViewModel
    {
        public void SetModel(object model)
        {
            Model = (MODEL) model;
        }

        public ViewModel GetModel()
        {
            return Model;
        }

        public MODEL Model{ get; set; }

        ViewModel IAltOxitePage.Model { get{ return Model; } }
    }
}