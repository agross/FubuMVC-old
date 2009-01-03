using AltOxite.Core.Domain;
using AltOxite.Core.Web.Html;

namespace AltOxite.Core.Web.DisplayModels
{
    public class LoginStatusDisplay
    {
        public LoginStatusDisplay(User user)
        {
            CurrentUser = user;
        }

        public User CurrentUser { get; private set; }
    }
}