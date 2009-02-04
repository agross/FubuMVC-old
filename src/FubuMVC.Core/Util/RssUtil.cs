using System.Collections.Generic;
using System.Text;
using System.Web;
using FubuMVC.Core.Controller;

namespace FubuMVC.Core.Util
{
    public class RssUtil
    {
        private const string feedHeaderTemplate = 
            "<rss version=\"2.0\" xmlns:dc=\"http://purl.org/dc/elements/1.1/\">" + 
            " <channel>" + 
            "  <title>{0}</title>" + 
            "  <description>{1}</description>" + 
            "  <link>{2}</link>" + 
            "  <language>{3}</language>" + 
            "  {4}" + // FeedItems
            " </channel>" +
            "</rss>";
        private const string feedItemTemplate = 
            "  <item>" + 
            "    <dc:creator>{0}</dc:creator>" + 
            "    <title>{1}</title>" + 
            "    <description>{2}</description>" + 
            "    <link>{3}</link>" + 
            "    <guid isPermaLink=\"true\">{4}</guid>" + 
            "    <pubDate>{5}</pubDate>" + 
            "    {6}" + // Categories
            "  </item>";
        private const string categoryTemplate = "    <category>{0}</category>";

        public string ToRss(Feed feed)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(
                feedHeaderTemplate.ToFormat(
                    HttpUtility.HtmlEncode(feed.Title),
                    HttpUtility.HtmlEncode(feed.Description),
                    feed.Link,
                    feed.Language,
                    CreateFeedITems(feed.GetFeedItems())));
            return stringBuilder.ToString();
        }

        private static string CreateFeedITems(IEnumerable<FeedItem> feedItems)
        {
            StringBuilder stringBuilder = new StringBuilder();
            feedItems.Each(feedItem =>
            {
                StringBuilder tagsStringBuilder = new StringBuilder();
                feedItem.GetTags().Each(tag => tagsStringBuilder.Append(categoryTemplate.ToFormat(tag)));
                    stringBuilder.Append(
                        feedItemTemplate.ToFormat(
                        HttpUtility.HtmlEncode(feedItem.Creator),
                        HttpUtility.HtmlEncode(feedItem.Title),
                        HttpUtility.HtmlEncode(feedItem.Description),
                        feedItem.Link,
                        feedItem.PremaLink,
                        feedItem.PublishDate,
                        tagsStringBuilder.ToString()));
            });
            return stringBuilder.ToString();
        }
    }
}