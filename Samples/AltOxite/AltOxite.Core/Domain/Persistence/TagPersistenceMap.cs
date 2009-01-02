namespace AltOxite.Core.Domain.Persistence
{
    public class TagPersistenceMap : DomainEntityMap<Tag>
    {
        public TagPersistenceMap()
        {
            Map(t => t.Name);
            Map(t => t.CreatedDate);
        }
    }
}