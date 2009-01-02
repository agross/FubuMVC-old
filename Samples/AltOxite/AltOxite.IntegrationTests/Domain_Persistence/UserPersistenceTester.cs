using AltOxite.Core.Domain;
using AltOxite.Core.Domain.Persistence;
using NUnit.Framework;

namespace AltOxite.IntegrationTests.Domain_Persistence
{
    [TestFixture]
    [Ignore("This test is failing because it is missing the mappings from Post, needs looking at")]
    public class UserPersistenceTester : PersistenceTesterContext<UserPersistenceMap, User>
    {
        [Test]
        public void should_load_and_save_a_user()
        {
            Specification
                .CheckProperty(u => u.Username, "username, anything here")
                .CheckProperty(u => u.DisplayName, "displayname, anything here")
                .CheckProperty(u => u.HashedEmail, "hashedemail, anything here")
                .CheckProperty(u => u.Password, "password, anything here")
                .CheckProperty(u => u.PasswordSalt, "salt, anything here")
                .CheckProperty(u => u.Status, 99)
                .CheckProperty(u => u.IsAnonymous, true)
                .VerifyTheMappings();
        }
    }
}