using System;
using System.Linq;
using AltOxite.Core.Domain;
using AltOxite.Core.Persistence;

namespace AltOxite.Core.Services
{
    public class SecurityDataService : ISecurityDataService
    {
        private readonly IRepository _repository;

        public SecurityDataService(IRepository repository)
        {
            _repository = repository;
        }

        public Guid? AuthenticateForUserId(string username, string password)
        {
            var user = _repository
                .Query<User>()
                .Where(u => u.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase) && u.Password == password)
                .FirstOrDefault();

            return user == null ? (Guid?) null : user.ID;
        }
    }
}