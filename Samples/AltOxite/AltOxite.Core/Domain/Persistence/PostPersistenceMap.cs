namespace AltOxite.Core.Domain.Persistence
{
    public class PostPersistenceMap : DomainEntityMap<Post>
    {
        public PostPersistenceMap()
        {
            Map(u => u.Title);
            Map(u => u.Class);
            Map(u => u.Published);
            Map(u => u.BodyShort);
        }
    }
}