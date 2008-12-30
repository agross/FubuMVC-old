namespace AltOxite.Core.Domain.Persistence
{
    public class CommentPersistenceMap : DomainEntityMap<Comment>
    {
        public CommentPersistenceMap()
        {
            Map(c => c.Author);
        }      
    }
}