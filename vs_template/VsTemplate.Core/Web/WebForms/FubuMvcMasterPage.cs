using System.Web.UI;

namespace VsTemplate.Core.Web.WebForms
{
    public class FubuMvcMasterPage : MasterPage, IFubuMvcPage
    {
        object IFubuMvcPage.Model{ get { return ((IFubuMvcPage) Page).Model; } }

        public ViewModel Model { get { return ((IFubuMvcPage) Page).Model as ViewModel; } }
        
        public void SetModel(object model)
        {
            throw new System.NotImplementedException();
        }
    }
}