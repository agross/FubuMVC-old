using System.Linq;
using AltOxite.Core.Domain;
using AltOxite.Core.Web.DisplayModels;
using FubuMVC.Core;
using Microsoft.Practices.ServiceLocation;

namespace AltOxite.Core.Web.Html
{
    public static class BlogPostHtmlExtension
    {
        public static string GetCommentsText(this IAltOxitePage viewPage, PostDisplay post)
        {
           var commentCount = (post.Comments == null) ? 0 : post.Comments.Count();
            return "<a href=\"{0}#comments\">{1}</a>"
                .ToFormat(
                    viewPage.UrlTo().PublishedPost(post),
                   (commentCount == 1)
                       ? "{0} comment".ToFormat(commentCount)
                       : "{0} comments".ToFormat(commentCount));
        }

        public static string GetGravatarImage(this IAltOxitePage viewPage, User user)
        {
            var siteConfig = ServiceLocator.Current.GetInstance<SiteConfiguration>();

            return "<img alt=\"{0} (gravatar)\" class=\"gravatar\" height=\"48\" src=\"{1}\" title=\"{0} (gravatar)\" width=\"48\" />"
                .ToFormat(
                    user.DisplayName, 
                    "http://www.gravatar.com/avatar/{0}?s=48&amp;default={1}".ToFormat(
                        user.HashedEmail, 
                        siteConfig.GravatarDefault));
        }

        public static string GetCommentPremalink(this IAltOxitePage viewPage, CommentDisplay comment)
        {
            return (!string.IsNullOrEmpty(comment.PermalinkHash)) 
                ? "<a href=\"#{0}\">{1}</a>".ToFormat(comment.PermalinkHash, comment.LocalPublishedDate)
                : "";
        }

        public static string GetCommentPremalinkBookmark(this IAltOxitePage viewPage, CommentDisplay comment)
        {
            return (!string.IsNullOrEmpty(comment.PermalinkHash)) 
                ? "<div><a name=\"{0}\"></a></div>".ToFormat(comment.PermalinkHash)
                : "";
        }

        public static string GetCommenterGravatarAndLink(this IAltOxitePage viewPage, CommentDisplay comment)
        {
            return (!string.IsNullOrEmpty(comment.User.Url))
                ? "<a class=\"avatar\" href=\"{0}\" rel=\"nofollow\">{1}</a>".ToFormat(comment.User.Url, viewPage.GetGravatarImage(comment.User))
                : viewPage.GetGravatarImage(comment.User);
        }

        public static string GetCommenterNameAndLink(this IAltOxitePage viewPage, CommentDisplay comment)
        {
            return (!string.IsNullOrEmpty(comment.User.Url))
                ? "<a href=\"{0}\">{1}</a>".ToFormat(comment.User.Url, comment.User.DisplayName)
                : comment.User.DisplayName;
        }

    }
}
