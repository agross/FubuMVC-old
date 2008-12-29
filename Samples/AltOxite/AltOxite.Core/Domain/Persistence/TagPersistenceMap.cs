namespace AltOxite.Core.Domain.Persistence
{
    public class TagPersistenceMap : DomainEntityMap<Tag>
    {
        public TagPersistenceMap()
        {
            Map(u => u.Name);
        }
    }
}