using System.IO;
using AltOxite.Core.Config;
using AltOxite.Core.Domain;
using AltOxite.Core.Domain.Persistence;
using AltOxite.Tests;
using FubuMVC.Core.Html;
using NUnit.Framework;

namespace AltOxite.IntegrationTests.Config
{
    [TestFixture]
    public class SQLiteSessionSourceConfiguration_IntegrationTester
    {
        private string _dbFileName;
        private SQLiteSessionSourceConfiguration _config;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            UrlContext.Stub("");
        }

        [SetUp]
        public void SetUp()
        {
            _dbFileName = Path.GetTempFileName();
            File.Delete(_dbFileName); // delete the file because we don't actually want it there, we want to simulate a DB creation
            _config = new SQLiteSessionSourceConfiguration(_dbFileName);
        }

        [TearDown]
        public void TearDown()
        {
            if (!File.Exists(_dbFileName)) return;

            File.Delete(_dbFileName);
        }

        [Test]
        public void CreateSessionSource_should_create_db_file()
        {
            _config.CreateSessionSource(new AltOxitePersistenceModel());
            File.Exists(_dbFileName).ShouldBeTrue();
        }

        [Test]
        public void CreateSessionSource_should_build_schema()
        {
            var testUsername = "TEST_USERNAME";
            var source = _config.CreateSessionSource(new AltOxitePersistenceModel());

            var userToSave = new User {Username = testUsername};
            using( var session = source.CreateSession() )
            {
                session.Save(userToSave);
                session.Flush();
            }

            using( var session = source.CreateSession() )
            {
                var loadedUser = session.Load<User>(userToSave.ID);
                loadedUser.ShouldNotBeNull();
                loadedUser.ID.ShouldEqual(userToSave.ID);
                loadedUser.Username.ShouldEqual(testUsername);
            }
        }
    }
}