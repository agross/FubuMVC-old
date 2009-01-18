using System.Web.UI;
using FubuMVC.Core.View;

namespace VsTemplate.Core.Web.WebForms
{
    public class FubuMvcPage<MODEL> : Page, IFubuMvcPage, IFubuView<MODEL> 
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

        object IFubuMvcPage.Model { get{ return Model; } }
    }
}