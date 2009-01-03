namespace AltOxite.Core.Domain.Persistence
{
    public class CommentPersistenceMap : DomainEntityMap<Comment>
    {
        public CommentPersistenceMap()
        {
            Map(c => c.Body);
            Map(c => c.Published);
            References(c => c.Post).CanNotBeNull().Cascade.All();
            References(c => c.User).CanNotBeNull().Cascade.All();
        }      
    }
}