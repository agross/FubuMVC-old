using AltOxite.Core.Domain;

namespace AltOxite.Core.Web.DisplayModels
{
    public class CommentDisplay
    {
        public CommentDisplay(Comment comment)
        {
            LocalPublishedDate = comment.Published.Value.ToString("MMMM dd, yyyy");
            PermalinkHash = comment.Published.Value.ToString("yyyyMMddhhmmssf");
            User = comment.User;
            Body = comment.Body;
            Post = comment.Post;
        }

        public string Body { get; private set; }
        public User User { get; private set; }
        public Post Post { get; private set; }
        public string LocalPublishedDate { get; private set; }
        public string PermalinkHash { get; private set; }
    }
}