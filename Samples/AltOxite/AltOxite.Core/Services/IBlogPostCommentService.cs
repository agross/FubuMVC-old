using AltOxite.Core.Domain;

namespace AltOxite.Core.Services
{
    public interface IBlogPostCommentService 
    {
        void AddCommentToBlogPost(string body, bool userSubscribed, User user, Post post);
    }
}