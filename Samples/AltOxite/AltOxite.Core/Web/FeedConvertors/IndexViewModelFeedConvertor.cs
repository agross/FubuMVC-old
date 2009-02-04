using AltOxite.Core.Web.Controllers;
using FubuMVC.Core;
using FubuMVC.Core.Controller;

namespace AltOxite.Core.Web.FeedConvertors
{
    public class IndexViewModelFeedConvertor : IFeedConverterFor<IndexViewModel>
    {
        public Feed ConvertModel(IndexViewModel model)
        {
            var feed = new Feed
            {
                Title = model.SiteName,
                Description = model.SiteName,
                Language = model.LanguageDefault,
                Link = "localhost"
            };
            model.Posts.Each(post =>
            {
                var feedItem = new FeedItem
                {
                    Creator = post.User.DisplayName,
                    Title = post.Title,
                    Description = post.Body,
                    Link = "",
                    PremaLink = "",
                    PublishDate = post.Published.ToShortDateString()
                };
                post.Tags.Each(tag => feedItem.AddTag(tag.Name));
                feed.AddFeedItem(feedItem);
            });
            return feed;
        }
    }
}