using System.Linq;
using AltOxite.Core.Domain;
using AltOxite.Core.Web.DisplayModels;
using FubuMVC.Core;
using Microsoft.Practices.ServiceLocation;

namespace AltOxite.Core.Web.Html
{
    public static class BlogPostHtmlExtension
    {
        public static string GetCommentsLink(this IAltOxitePage viewPage, PostDisplay post)
        {
            var commentCount = post.CommentsCount;
            return "<a href=\"{0}#comments\">{1}</a>"
                .ToFormat(
                    viewPage.UrlTo().PublishedPost(post),
                   (commentCount == 1)
                       ? LocalizationManager.GetTextForKey("{0} comment").ToFormat(commentCount)
                       : LocalizationManager.GetTextForKey("{0} comments").ToFormat(commentCount));
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
    }
}