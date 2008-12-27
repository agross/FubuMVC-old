using FluentNHibernate.Mapping;

namespace AltOxite.Core.Domain.Persistence
{
    public class UserPersistenceMap : DomainEntityMap<User>
    {
        public UserPersistenceMap()
        {
            Map(u => u.Username);
            Map(u => u.DisplayName);
            Map(u => u.HashedEmail);
            Map(u => u.Password);
            Map(u => u.PasswordSalt);
            Map(u => u.Status);
            Map(u => u.IsAnonymous);
        }
    }
}