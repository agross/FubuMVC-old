namespace AltOxite.Core.Domain.Persistence
{
    public sealed class UserPersistenceMap : DomainEntityMap<User>
    {
        public UserPersistenceMap()
        {
            NotLazyLoaded();

            Map(u => u.Username);
            Map(u => u.DisplayName);
            Map(u => u.HashedEmail);
            Map(u => u.Email);
            Map(u => u.Url);
            Map(u => u.Password);
            Map(u => u.PasswordSalt);
            Map(u => u.Status);
            Map(u => u.IsAnonymous);
            HasMany<Post>(u => u.Posts).AsBag().Cascade.SaveUpdate().IsInverse();
        }
    }
}