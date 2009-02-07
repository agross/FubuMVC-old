namespace AltOxite.Core.Domain.Persistence
{
    public sealed class UserPersistenceMap : DomainEntityMap<User>
    {
        public UserPersistenceMap()
        {
            MapEntity();
        }

        private void MapEntity() 
        {
            Not.LazyLoad();

            Map(u => u.Username);
            Map(u => u.DisplayName);
            Map(u => u.HashedEmail);
            Map(u => u.Email);
            Map(u => u.Url);
            Map(u => u.Password);
            Map(u => u.PasswordSalt);
            Map(u => u.Status);
            Map(u => u.UserRole);
            Map(u => u.Remember);
            //Map(u => u.IsAuthenticated); // Does not have to be persisted
            HasMany(u => u._posts).AsBag().Cascade.SaveUpdate().Inverse();
        }
    }
}