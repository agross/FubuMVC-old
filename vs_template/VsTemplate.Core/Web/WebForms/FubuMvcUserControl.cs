using System.Web.UI;
using FubuMVC.Core.View;

namespace VsTemplate.Core.Web.WebForms
{
    public class FubuMvcUserControl<MODEL> : UserControl, IFubuMvcPage, IFubuView<MODEL> 
        where MODEL : class
    {
        public void SetModel(object model)
        {
            Model = (MODEL) model;
        }

        public object GetModel()
        {
            return Model;
        }

        public MODEL Model{ get; set; }

        object IFubuMvcPage.Model { get { return Model; } }
    }
}