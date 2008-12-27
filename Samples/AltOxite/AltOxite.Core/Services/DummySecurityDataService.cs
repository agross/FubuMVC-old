using System;
using System.Linq;
using AltOxite.Core.Domain;
using AltOxite.Core.Persistence;

namespace AltOxite.Core.Services
{
    public class DummySecurityDataService : ISecurityDataService
    {
        object TODO_IMPLEMENT_REAL_DATA_ACCESS____THEN_DELETE_THIS_FILE = null;
        private readonly IRepository _repository;

        public DummySecurityDataService(IRepository repository)
        {
            _repository = repository;
        }

        public Guid? AuthenticateForUserId(string username, string password)
        {
            //TODO: Dummy, just authenticate everyone the first time
            var user = _repository.Query<User>().Where(u => u.Username == username).SingleOrDefault();

            if( user == null )
            {
                user = new User
                    {
                        ID = Guid.NewGuid(),
                        DisplayName = username,
                        Username = username,
                        Password = password
                    };

                _repository.Save(user);
            }

            return user.ID;
        }
    }
}