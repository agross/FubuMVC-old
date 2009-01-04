using AltOxite.Core.Domain;

namespace AltOxite.Core.Web.DisplayModels
{
    public class CommentFormDisplay
    {
        public CommentFormDisplay(User user, Comment comment)
        {
            User = user;
            Body = comment.Body;
            UserSubscribed = comment.UserSubscribed;
        }

        public User User { get; private set; }
        public string Body { get; private set; }
        public bool UserSubscribed { get; private set; }
    }
}