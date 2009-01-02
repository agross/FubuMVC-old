using System.Web.UI;

namespace AltOxite.Core.Web.WebForms
{
    public class AltOxiteMasterPage : MasterPage, IAltOxitePage
    {
        object IAltOxitePage.Model{ get { return ((IAltOxitePage) Page).Model; } }

        public ViewModel Model { get { return ((IAltOxitePage) Page).Model as ViewModel; } }
        
        public void SetModel(object model)
        {
            throw new System.NotImplementedException();
        }
    }
}