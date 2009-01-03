using System;
using System.Collections.Generic;
using System.Linq;

namespace AltOxite.Core.Domain
{
    public class Post : DomainEntity
    {
        public virtual string Title { get; set; }
        public virtual DateTime? Published { get; set; }
        public virtual string BodyShort { get; set; }
        public virtual string Body { get; set; }
        public virtual string Slug { get; set; }
        public virtual User User { get; set; }

        public virtual IList<Tag> Tags { get; set; }  // TODO: make these private, add "AddTag/RemoveTag" type methods
        public virtual IList<Comment> Comments { get; set; } // TODO: make these private, add "AddComment/RemoveComment" type methods

        public virtual void AddTag(Tag tag)
        {
            if (Tags == null) Tags = new List<Tag>();

            Tags.Add(tag);
        }
        public virtual void RemoveTag(Tag tag)
        {
            if (Tags == null) Tags = new List<Tag>();

            if (Tags.Contains(tag)) Tags.Remove(tag);
        }
        public virtual IEnumerable<Tag> GetTags()
        {
            if (Tags == null) Tags = new List<Tag>();

            return Tags.ToArray();
        }

        public virtual void AddComment(Comment comment)
        {
            if (Comments == null) Comments = new List<Comment>();

            Comments.Add(comment);
        }
        public virtual void RemoveComment(Comment comment)
        {
            if (Comments == null) Comments = new List<Comment>();

            if (Comments.Contains(comment)) Comments.Remove(comment);
        }
        public virtual IEnumerable<Comment> GetComments()
        {
            if (Comments == null) Comments = new List<Comment>();

            return Comments.ToArray();
        }

        ///////// Properties left from originalOxite source
        
        //public virtual IPost Parent { get; }
        //public virtual Guid CreatorUserID { get; set; }
        //public virtual IUser CreatorUser { get; }

        //public virtual byte State { get; set; }
        //public virtual DateTime? Created { get; set; }
        //public virtual DateTime? Modified { get; set; }
        //public virtual string SearchBody { get; set;  }
        //public virtual IArea Area { get; }
    }
}