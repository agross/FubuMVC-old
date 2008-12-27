using AltOxite.Core.Domain;
using AltOxite.Core.Domain.Persistence;
using NUnit.Framework;

namespace AltOxite.IntegrationTests.Domain_Persistence
{
    [TestFixture]
    public class UserPersistenceTester : PersistenceTesterContext<UserPersistenceMap, User>
    {
        [Test]
        public void should_load_and_save_a_user()
        {
            Specification
                .CheckProperty(u => u.Username, "anything here")
                .CheckProperty(u => u.DisplayName, "anything here")
                .CheckProperty(u => u.HashedEmail, "anything here")
                .CheckProperty(u => u.Password, "anything here")
                .CheckProperty(u => u.PasswordSalt, "anything here")
                .CheckProperty(u => u.Status, 99)
                .CheckProperty(u => u.IsAnonymous, true)
                .VerifyTheMappings();
        }
    }
}