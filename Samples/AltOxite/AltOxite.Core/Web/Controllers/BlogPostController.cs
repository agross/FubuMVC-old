using System;
using System.Linq;
using System.Text;
using AltOxite.Core.Domain;
using AltOxite.Core.Persistence;
using FubuMVC.Core;

namespace AltOxite.Core.Web.Controllers
{
    public class BlogPostController
    {
        private readonly IRepository _repository;

        public BlogPostController(IRepository repository)
        {
            _repository = repository;
        }

        public BlogPostViewModel Index(BlogPostSetupViewModel inModel)
        {
            if( inModel.Slug.IsEmpty()) return new BlogPostViewModel();

            var post = _repository.Query<Post>().Where(p => p.Slug == inModel.Slug).SingleOrDefault();

            if (post == null) return new BlogPostViewModel();

            var tagLinks = new StringBuilder();
            if (post.Tags != null)
                post.Tags.Each(tag => tagLinks.Append("<a href=\"{0}\">{1}</a> ".ToFormat("#", tag.Name)));

            var commentCount = 0;
            if (post.Comments != null)
                commentCount = post.Comments.Count();

            var tagLinksAndCommentsLink = ((tagLinks.Length != 0) ? LocalizationManager.GetTextForKey("Filed under {0}| ").ToFormat(tagLinks) : "") + 
                "<a href=\"{0}\">{1}</a> ".ToFormat("linktopostcomments", (commentCount + ((commentCount == 1) ? LocalizationManager.GetTextForKey(" comment") : LocalizationManager.GetTextForKey(" comments")))) +
                "<a href=\"{0}\" class=\"arrow\">&raquo;</a>".ToFormat("linktopost");

            var css = ((inModel.CurrentPostOnPage == 1) ? "first " : "") +
                      ((inModel.CurrentPostOnPage == inModel.TotalPostsOnPage) ? "last" : "");

            return new BlogPostViewModel
            {
                Post = post,
                LocalPublishedDate = post.Published.Value.ToLongDateString(), //To local time
                TagLinksAndCommentsLink = tagLinksAndCommentsLink,
                Class = (css != "") ? " class=\"{0}\"".ToFormat(css.Trim()) : ""
            };
        }
    }

    public class BlogPostSetupViewModel
    {
        public int PostYear { get; set; }
        public int PostMonth  { get; set; }
        public int PostDay  { get; set; }
        public string Slug { get; set; }
        public int TotalPostsOnPage { get; set; }
        public int CurrentPostOnPage { get; set; }
    }

    [Serializable]
    public class BlogPostViewModel : ViewModel
    {
        public Post Post { get; set; }
        public string LocalPublishedDate { get; set; }
        public string TagLinksAndCommentsLink { get; set; }
        public string Class { get; set; }
    }
}