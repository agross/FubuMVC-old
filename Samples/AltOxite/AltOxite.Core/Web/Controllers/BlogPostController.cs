using System;
using System.Linq;
using System.Web.Security;
using AltOxite.Core.Domain;
using AltOxite.Core.Persistence;
using AltOxite.Core.Web.DisplayModels;
using FubuMVC.Core;
using FubuMVC.Core.Controller.Config;

namespace AltOxite.Core.Web.Controllers
{
    public class BlogPostController
    {
        private readonly IRepository _repository;

        public BlogPostController(IRepository repository)
        {
            _repository = repository;
        }

        public BlogPostViewModel Index(BlogPostViewModel inModel)
        {
            if (inModel.Slug.IsEmpty()) return new BlogPostViewModel();

            var post = _repository.Query<Post>().Where(p => p.Slug == inModel.Slug).SingleOrDefault();

            User user = inModel.CurrentUser;
            if (inModel.Comment != null && !string.IsNullOrEmpty(inModel.Comment.Body))
            {
                user = _repository.Query<User>().Where(p => p.Email.ToLower() == inModel.Comment.User.Email.ToLower()).SingleOrDefault() 
                    ?? new User
                       {
                           IsAuthenticated = false, 
                           UserRole = UserRoles.NotAuthenticated, 
                           Email = inModel.Comment.User.Email,
                           HashedEmail = GenerateGravatarHash(inModel.Comment.User.Email)
                       };
                if (user.UserRole == UserRoles.NotAuthenticated)
                {
                    user.DisplayName = inModel.Comment.User.DisplayName;
                    user.Url = inModel.Comment.User.Url;
                }
                _repository.Save(user);

                Comment comment = new Comment
                {
                    Body = inModel.Comment.Body,
                    User = user,
                    Post = post,
                    UserSubscribed = inModel.Comment.UserSubscribed
                };
                _repository.Save(comment);
            
                // Reload post with new comment
                post = _repository.Load<Post>(post.ID);
            }

            var postDisplay = (post == null) ? null : new PostDisplay(post);
            return new BlogPostViewModel
            {
                Post = postDisplay,
                Comment = new CommentFormDisplay(user, new Comment(), postDisplay)
            };
        }

        private static string GenerateGravatarHash(string emailAddress)
        {
            var hash = FormsAuthentication.HashPasswordForStoringInConfigFile(emailAddress.Trim().ToLower(), "MD5");
            return (hash != null) ? hash.Trim().ToLower() : "";
        }
    }

    [Serializable]
    public class BlogPostViewModel : ViewModel
    {
        [Required] public int PostYear { get; set; }
        [Required] public int PostMonth { get; set; }
        [Required] public int PostDay { get; set; }
        [Required] public string Slug { get; set; }
        public PostDisplay Post { get; set; }
        public CommentFormDisplay Comment { get; set; }
    }
}