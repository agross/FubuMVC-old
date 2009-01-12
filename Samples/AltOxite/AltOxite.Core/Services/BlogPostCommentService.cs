using System;
using AltOxite.Core.Domain;
using AltOxite.Core.Persistence;

namespace AltOxite.Core.Services
{
    public class BlogPostCommentService : IBlogPostCommentService
    {
        private readonly IRepository _repository;

        public BlogPostCommentService(IRepository repository)
        {
            _repository = repository;
        }

        public void AddCommentToBlogPost(string body, bool userSubscribed, User user, Post post)
        {
            var comment = new Comment
            {
                Body = body,
                User = user,
                Post = post,
                UserSubscribed = userSubscribed
            };

            //TODO: Need to implement publishing/pending stuff
            comment.Published = DateTime.UtcNow;

            post.AddComment(comment);
            _repository.Save(post);
        }
    }
}