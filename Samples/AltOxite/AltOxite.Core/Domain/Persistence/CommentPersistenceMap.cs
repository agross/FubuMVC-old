namespace AltOxite.Core.Domain.Persistence
{
    public class CommentPersistenceMap : DomainEntityMap<Comment>
    {
        public CommentPersistenceMap()
        {
            MapEntity();
        }

        private void MapEntity() 
        {
            Map(c => c.Body);
            Map(c => c.Published);
            Map(c => c.UserSubscribed);
            References(c => c.Post).Not.Nullable().Cascade.All();
            References(c => c.User).Not.Nullable().Cascade.All();
        }
    }
}