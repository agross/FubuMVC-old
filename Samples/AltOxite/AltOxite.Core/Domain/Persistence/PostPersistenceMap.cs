namespace AltOxite.Core.Domain.Persistence
{
    public sealed class PostPersistenceMap : DomainEntityMap<Post>
    {
        public PostPersistenceMap()
        {
            Map(u => u.Title);
            Map(u => u.BodyShort);
            Map(u => u.Body);
            Map(u => u.Slug).WithUniqueConstraint();
            Map(u => u.Published);
            References(u => u.User).Cascade.All();
            HasManyToMany<Tag>(u => u.Tags).WithTableName("PostsToTags").Cascade.All();
            HasMany<Comment>(u => u.Comments).Cascade.All();
        }
    }
}