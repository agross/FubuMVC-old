namespace AltOxite.Core.Domain
{
    public class Comment : DomainEntity
    {
        public virtual string Author { get; set; }
        public virtual Post Post { get; set; }
    }
}