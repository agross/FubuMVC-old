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

        public static string GetGravatarImage(this IAltOxitePage viewPage, User user, string gravatarDefault)
        {
            return "<img alt=\"{0} (gravatar)\" class=\"gravatar\" height=\"48\" src=\"{1}\" title=\"{0} (gravatar)\" width=\"48\" />"
                .ToFormat(
                    user.DisplayName, 
                    "http://www.gravatar.com/avatar/{0}?s=48&amp;default={1}".ToFormat(
                        user.HashedEmail, 
                        gravatarDefault));
        }
    }
}