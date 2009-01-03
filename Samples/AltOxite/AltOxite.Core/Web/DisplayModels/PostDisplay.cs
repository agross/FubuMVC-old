using System;
using System.Collections.Generic;
using System.Linq;
using AltOxite.Core.Domain;

namespace AltOxite.Core.Web.DisplayModels
{
    public class PostDisplay
    {
        public PostDisplay(Post post)
        {
            Published = post.Published.GetValueOrDefault(DateTime.MinValue);
            LocalPublishedDate = Published.ToLongDateString(); //TODO: To local time
            Slug = post.Slug;
            Comments = post.GetComments().Select(c => new CommentDisplay(c));
            CommentsCount = post.GetComments().Count();
            Title = post.Title;
            BodyShort = post.BodyShort;
            Body = post.Body;
            Tags = post.GetTags().OrderByDescending(t => t.CreatedDate).Select(t => new TagDisplay(t)); //TODO: this (OrderByDescending) is business logic and needs to be moved outta here most likely
            User = post.User;
        }

        public DateTime Published { get; set; }
        public string LocalPublishedDate { get; set; }
        public string Slug { get; set; }
        public int CommentsCount { get; set; }
        public string Title { get; set; }
        public string BodyShort { get; set; }
        public string Body { get; set; }

        public IEnumerable<CommentDisplay> Comments { get; set; } //TODO: Make this 'CommentDisplay' or something
        public IEnumerable<TagDisplay> Tags { get; set; } //TODO: Make this 'TagDisplay' or something
        public User User { get; set; } //TODO: Make this 'UserDisplay' or something
    }
}