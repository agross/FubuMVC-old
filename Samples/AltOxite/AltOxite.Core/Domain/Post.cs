using System;
using System.Collections.Generic;
using System.Web;
using FubuMVC.Core;

namespace AltOxite.Core.Domain
{
    public class Post : DomainEntity
    {
//        IPost Parent { get; }
//        Guid ID { get; set; }
//        Guid CreatorUserID { get; set; }
//        IUser CreatorUser { get; }
        public string Title { get; set; }
        public string Class { get; set; }
        public DateTime? Published { get; set; }
        public string BodyShort { get; set; }

        //string Body { get; set; }
        //string BodyShort { get; set; }
        //byte State { get; set; }
        //string Slug { get; set; }
        //DateTime? Created { get; set; }
        //DateTime? Modified { get; set; }
        //DateTime? Published { get; set; }
        //string SearchBody { get; set;  }
//        IArea Area { get; }
//        IEnumerable<ITag> Tags { get; }
    }
}