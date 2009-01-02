using System;

namespace AltOxite.Core.Domain
{
    public class Tag : DomainEntity
    {
        public virtual string Name { get; set; }
        public virtual DateTime CreatedDate { get; set; }
    }
}