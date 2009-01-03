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
            References(u => u.User).Cascade.SaveUpdate();
            HasManyToMany<Tag>(u => u._tags).WithTableName("PostsToTags").Cascade.SaveUpdate();
            HasMany<Comment>(u => u._comments).Cascade.All().IsInverse();
        }
    }
}