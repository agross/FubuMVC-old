using System;
using System.Collections.Generic;
using System.Linq;

namespace AltOxite.Core.Domain
{
    public class Post : DomainEntity
    {
        public virtual IList<Tag> _tags { get; set; } // = new List<Tag>();  // TODO: make these private, add "AddTag/RemoveTag" type methods
        public virtual IList<Comment> _comments { get; set; } // = new List<Comment>(); // TODO: make these private, add "AddComment/RemoveComment" type methods

        public Post() // TODO: Probably o be removed later when these lists are private
        {
            _tags = new List<Tag>();
            _comments = new List<Comment>();
        }

        public virtual string Title { get; set; }
        public virtual DateTime? Published { get; set; }
        public virtual string BodyShort { get; set; }
        public virtual string Body { get; set; }
        public virtual string Slug { get; set; }
        public virtual User User { get; set; }

        public virtual void AddTag(Tag tag)
        {
            _tags.Add(tag);
        }
        public virtual void RemoveTag(Tag tag)
        {
            _tags.Remove(tag);
        }
        public virtual IEnumerable<Tag> GetTags()
        {
            return _tags.AsEnumerable();
        }

        public virtual void AddComment(Comment comment)
        {
            _comments.Add(comment);
        }
        public virtual void RemoveComment(Comment comment)
        {
            _comments.Remove(comment);
        }
        public virtual IEnumerable<Comment> GetComments()
        {
            return _comments.AsEnumerable();
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