using AltOxite.Core.Domain;

namespace AltOxite.Core.Web.DisplayModels
{
    public class CommentFormDisplay
    {
        public CommentFormDisplay(User user)
        {
            User = user;
        }

        public User User { get; private set; }
    }
}