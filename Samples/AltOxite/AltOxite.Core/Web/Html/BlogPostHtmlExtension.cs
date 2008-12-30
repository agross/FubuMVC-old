using System.Linq;
using AltOxite.Core.Domain;
using FubuMVC.Core;

namespace AltOxite.Core.Web.Html
{
    public static class BlogPostHtmlExtension
    {
        public static string GetCssForLiTag(this IAltOxitePage viewPage, int currentPost, int totalPosts)
        {
            var css = ((currentPost == 1) ? "first " : "") + ((currentPost == totalPosts) ? "last" : "");
            return (css == "") ? "" : " class=\"{0}\"".ToFormat(css.Trim());
        }

        public static string GetCommentsLink(this IAltOxitePage viewPage, Post post)
        {
            var commentCount = (post.Comments == null) ? 0 : post.Comments.Count();
            return "<a href=\"{0}#comments\">{1}</a>"
                .ToFormat(
                    viewPage.UrlTo().Post(post),
                   (commentCount == 1)
                       ? LocalizationManager.GetTextForKey("{0} comment").ToFormat(commentCount)
                       : LocalizationManager.GetTextForKey("{0} comments").ToFormat(commentCount));
        }
    }
}