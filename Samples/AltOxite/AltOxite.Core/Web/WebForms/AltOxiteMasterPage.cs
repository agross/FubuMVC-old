using System.Web.UI;

namespace AltOxite.Core.Web.WebForms
{
    public class AltOxiteMasterPage : MasterPage, IAltOxitePage
    {
        public ViewModel Model{ get { return ((IAltOxitePage) Page).Model; } }
        
        public void SetModel(object model)
        {
            throw new System.NotImplementedException();
        }
    }
}