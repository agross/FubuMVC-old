using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Security;
using AltOxite.Core.Domain;
using AltOxite.Core.Persistence;
using AltOxite.Core.Web.DisplayModels;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Controller;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Controller.Results;
using FubuMVC.Core.Util;

namespace AltOxite.Core.Web.Controllers
{
    public class BlogPostController
    {
        private readonly IRepository _repository;
        private readonly IUrlResolver _resolver;

        public BlogPostController(IRepository repository, IUrlResolver resolver)
        {
            _repository = repository;
            _resolver = resolver;
        }

        public BlogPostViewModel Index(BlogPostViewModel inModel)
        {
            var badRedirectResult = new BlogPostViewModel { ResultOverride = new RedirectResult(_resolver.PageNotFound()) };

            if (inModel.Slug.IsEmpty()) return badRedirectResult;

            var post = _repository.Query(new PostBySlug(inModel.Slug)).SingleOrDefault();

            if (post == null) return badRedirectResult;

            User user = inModel.CurrentUser;
            
            var postDisplay = new PostDisplay(post);
            return new BlogPostViewModel
            {
                Post = postDisplay,
                Comment = new CommentFormDisplay(user, new Comment(), postDisplay)
            };
        }

        public BlogPostViewModel Comment(BlogPostCommentViewModel inModel)
        {
            var badRedirectResult = new BlogPostViewModel{ResultOverride = new RedirectResult(_resolver.PageNotFound())};

            //TODO: What if the referenced post doesn't exist?
            if (inModel.Slug.IsEmpty()) return badRedirectResult;

            var post = _repository.Query(new PostBySlug(inModel.Slug)).SingleOrDefault();

            if (post == null) return badRedirectResult;

            var result = new BlogPostViewModel();

            valid_comment_submission(inModel, result);

            if( result.InvalidFields.Count > 0 ) return result;

            //TODO: What if the referenced post doesn't exist?

            var user = _repository.Query(new UserByEmail(inModel.UserEmail)).SingleOrDefault();
            
            //TODO: There's just way too much going on here, need to move this out of here 
            // Domain service perhaps?
            if( user == null )
            {
                user = new User
                           {
                               Username = inModel.UserDisplayName,
                               IsAuthenticated = false,
                               UserRole = UserRoles.NotAuthenticated,
                               Email = inModel.UserEmail,
                               HashedEmail = GenerateGravatarHash(inModel.UserEmail)
                           };
            }

            if( ! user.IsAuthenticated )
            {
                user.DisplayName = inModel.UserDisplayName;
                user.Url = inModel.UserUrl;
            }

            _repository.Save(user);

             
            var comment = new Comment
            {
                Body = inModel.Body,
                User = user,
                Post = post,
                UserSubscribed = inModel.UserSubscribed
            };

            //TODO: Need to implement publishing/pending stuff
            comment.Published = DateTime.UtcNow;

            post.AddComment(comment);
            _repository.Save(post);

            var postDisplay = new PostDisplay(post);

            result.ResultOverride = new RedirectResult(_resolver.PublishedPost(postDisplay));

            return result;
        }

        private static void valid_comment_submission(BlogPostCommentViewModel inModel, BlogPostViewModel result)
        {
            // TODO: have attributes on the viewmodel and do validation elsewhere 
            if (inModel.UserDisplayName.IsEmpty()) result.AddInvalidField<BlogPostCommentViewModel>(x => x.UserDisplayName);
            if (inModel.UserEmail.IsEmpty()) result.AddInvalidField<BlogPostCommentViewModel>(x => x.UserEmail);
            if (inModel.Body.IsEmpty()) result.AddInvalidField<BlogPostCommentViewModel>(x => x.Body);
        }

        private static string GenerateGravatarHash(string emailAddress)
        {
            var hash = FormsAuthentication.HashPasswordForStoringInConfigFile(emailAddress.Trim().ToLower(), "MD5");
            return (hash != null) ? hash.Trim().ToLower() : "";
        }
    }

    [Serializable]
    public class BlogPostViewModel : ViewModel, ISupportResultOverride
    {
        public BlogPostViewModel()
        {
            InvalidFields = new List<string>();
        }

        public void AddInvalidField<MODEL>(Expression<Func<MODEL, object>> fieldExpression)
        {
            var name = ReflectionHelper.GetAccessor(fieldExpression).Name;
            InvalidFields.Add(name);
        }

        public IList<string> InvalidFields { get; set; }
        public IInvocationResult ResultOverride { get; set; }

        [Required] public int PostYear { get; set; }
        [Required] public int PostMonth { get; set; }
        [Required] public int PostDay { get; set; }
        [Required] public string Slug { get; set; }
        public PostDisplay Post { get; set; }
        public CommentFormDisplay Comment { get; set; }
    }

    [Serializable]
    public class BlogPostCommentViewModel : ViewModel
    {
        [Required] public int PostYear { get; set; }
        [Required] public int PostMonth { get; set; }
        [Required] public int PostDay { get; set; }
        [Required] public string Slug { get; set; }
        public string UserDisplayName { get; set; }
        public string UserEmail { get; set; }
        public string Body { get; set; }
        public string UserUrl { get; set; }
        public bool Remember { get; set; }
        public bool UserSubscribed { get; set; }
    }
}