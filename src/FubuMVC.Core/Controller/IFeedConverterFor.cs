using System.Collections.Generic;
using System.Linq;

namespace FubuMVC.Core.Controller
{
    public interface IFeedConverterFor<MODEL>
    {
        Feed ConvertModel(MODEL model);
    }

    public class Feed
    {
        private readonly IList<FeedItem> _feedItems = new List<FeedItem>();

        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string Language { get; set; }

        public void AddFeedItem(FeedItem feedItem)
        {
            _feedItems.Add(feedItem);
        }
        public IEnumerable<FeedItem> GetFeedItems()
        {
            return _feedItems.ToList();
        }
    }

    public class FeedItem
    {
        private readonly IList<string> _tags = new List<string>();

        public string Creator { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string PremaLink { get; set; }
        public string PublishDate { get; set; }

        public void AddTag(string tag)
        {
            _tags.Add(tag);
        }
        public IEnumerable<string> GetTags()
        {
            return _tags.ToList();
        }
    }
}