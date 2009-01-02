namespace AltOxite.Core.Domain.Persistence
{
    public class CommentPersistenceMap : DomainEntityMap<Comment>
    {
        public CommentPersistenceMap()
        {
            Map(c => c.Author);
            References(c => c.Post).CanNotBeNull().Cascade.All();
        }      
    }
}