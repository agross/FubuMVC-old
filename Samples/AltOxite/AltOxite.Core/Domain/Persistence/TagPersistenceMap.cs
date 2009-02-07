namespace AltOxite.Core.Domain.Persistence
{
    public class TagPersistenceMap : DomainEntityMap<Tag>
    {
        public TagPersistenceMap()
        {
            MapEntity();
        }

        private void MapEntity() 
        {
            Map(t => t.Name);
            Map(t => t.CreatedDate);
        }
    }
}