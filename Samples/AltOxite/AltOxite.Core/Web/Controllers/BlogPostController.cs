using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AltOxite.Core.Domain;
using AltOxite.Core.Persistence;
using AltOxite.Core.Services;
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
        private readonly IBlogPostCommentService _blogPostCommentService;
        private readonly IUserService _userService;

        public BlogPostController(IRepository repository, IUrlResolver resolver, IBlogPostCommentService blogPostCommentService, IUserService userService)
        {
            _repository = repository;
            _resolver = resolver;
            _blogPostCommentService = blogPostCommentService;
            _userService = userService;
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

            if (inModel.Slug.IsEmpty()) return badRedirectResult;

            var post = _repository.Query(new PostBySlug(inModel.Slug)).SingleOrDefault();

            if (post == null) return badRedirectResult;

            var result = new BlogPostViewModel();

            valid_comment_submission(inModel, result);

            if( result.InvalidFields.Count > 0 )
            {
                result.ResultOverride = new RedirectResult(_resolver.PublishedPost(new PostDisplay(post)));
                return result;
            }

            var user = _userService.AddOrUpdateUser(inModel.UserEmail, inModel.UserDisplayName, inModel.UserUrl);
             
            _blogPostCommentService.AddCommentToBlogPost(inModel.Body, inModel.UserSubscribed, user, post);

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