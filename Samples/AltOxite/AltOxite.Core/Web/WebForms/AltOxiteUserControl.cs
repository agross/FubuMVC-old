using System.Web.UI;
using FubuMVC.Core.View;

namespace AltOxite.Core.Web.WebForms
{
    public class AltOxiteUserControl<MODEL> : UserControl, IAltOxitePage, IFubuView<MODEL> 
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

        object IAltOxitePage.Model { get { return Model; } }
    }
}