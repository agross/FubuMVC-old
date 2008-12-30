using System;
using System.Collections.Generic;

namespace AltOxite.Core.Domain
{
    public class Post : DomainEntity
    {
        public virtual string Title { get; set; }
        public virtual DateTime? Published { get; set; }
        public virtual string BodyShort { get; set; }
        public virtual string Body { get; set; }
        public virtual IEnumerable<Tag> Tags { get; set; }
        public virtual IEnumerable<Comment> Comments { get; set; }
        public virtual string Slug { get; set; }

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