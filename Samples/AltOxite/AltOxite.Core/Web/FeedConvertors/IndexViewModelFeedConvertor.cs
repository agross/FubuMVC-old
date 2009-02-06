using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using AltOxite.Core.Web.Controllers;
using FubuMVC.Core;
using FubuMVC.Core.Controller;
using FubuMVC.Core.Controller.Config;
using FubuMVC.Core.Routing;

namespace AltOxite.Core.Web.FeedConvertors
{
    public class IndexViewModelFeedConvertor : IFeedConverterFor<IndexViewModel>
    {
        public ICurrentRequest CurrentRequest { get; set; }
        private readonly IUrlResolver _urlResolver;
        private readonly ICurrentRequest _currentRequest;

        public IndexViewModelFeedConvertor(IUrlResolver urlResolver, ICurrentRequest currentRequest)
        {
            CurrentRequest = currentRequest;
            _urlResolver = urlResolver;
            _currentRequest = currentRequest;
        }

        public bool TryConvertModel(IndexViewModel model, out SyndicationFeed syndicationFeed)
        {
            syndicationFeed = new SyndicationFeed(
                model.SiteName, 
                model.SiteName, 
                _currentRequest.GetUrl(),
                "AllPostsFeed",
                new DateTimeOffset(DateTime.Now))
            {
                Description = new TextSyndicationContent(model.SiteName)
            };

            List<SyndicationItem> feedItems = new List<SyndicationItem>();
            model.Posts.Each(post =>
            {
                var feedItem = new SyndicationItem(
                    post.Title,
                    post.Body,
                    new Uri(_urlResolver.PublishedPost(post).ToFullUrl()),
                    new Uri(_urlResolver.PublishedPost(post).ToFullUrl()).ToString(),
                    new DateTimeOffset(post.Published));

                feedItem.Authors.Add(new SyndicationPerson(post.User.Email, post.User.DisplayName, post.User.Url));
                post.Tags.Each(tag => feedItem.Categories.Add(new SyndicationCategory(tag.Name)));
                feedItems.Add(feedItem);
            });
            syndicationFeed.Items = feedItems.AsEnumerable();

            return true;
        }
    }
}