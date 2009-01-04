using System;

namespace AltOxite.Core.Domain
{
    public class Comment : DomainEntity
    {
        public virtual string Body { get; set; }
        public virtual User User { get; set; }
        public virtual Post Post { get; set; }
        public virtual DateTime? Published { get; set; }
        public virtual bool UserSubscribed { get; set; }
    }
}