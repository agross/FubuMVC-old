using System;
using System.Linq;
using System.Text;
using AltOxite.Core.Domain;
using FubuMVC.Core;

namespace AltOxite.Core.Web.Controllers
{
    public class BlogPostController
    {
        public BlogPostViewModel Index(BlogPostSetupViewModel inModel)
        {
            if (inModel.Post == null) return new BlogPostViewModel();

            var tagLinks = new StringBuilder();
            if (inModel.Post.Tags != null)
                inModel.Post.Tags.Each(tag => tagLinks.Append("<a href=\"{0}\">{1}</a> ".ToFormat("#", tag.Name)));

            var commentCount = 0;
            if (inModel.Post.Comments != null)
                commentCount = inModel.Post.Comments.Count();

            var tagLinksAndCommentsLink = ((tagLinks.Length != 0) ? LocalizationManager.GetTextForKey("Filed under {0}| ").ToFormat(tagLinks) : "") + 
                "<a href=\"{0}\">{1}</a> ".ToFormat("linktopostcomments", (commentCount + ((commentCount == 1) ? LocalizationManager.GetTextForKey(" comment") : LocalizationManager.GetTextForKey(" comments")))) +
                "<a href=\"{0}\" class=\"arrow\">&raquo;</a>".ToFormat("linktopost");

            var css = ((inModel.CurrentPostOnPage == 1) ? "first " : "") +
                      ((inModel.CurrentPostOnPage == inModel.TotalPostsOnPage) ? "last" : "");

            return new BlogPostViewModel
            {
                Post = inModel.Post,
                LocalPublishedDate = inModel.Post.Published.Value.ToLongDateString(), //To local time
                TagLinksAndCommentsLink = tagLinksAndCommentsLink,
                Class = "class=\"{0}\"".ToFormat(css)
            };
        }
    }

    public class BlogPostSetupViewModel
    {
        public Post Post { get; set; }
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