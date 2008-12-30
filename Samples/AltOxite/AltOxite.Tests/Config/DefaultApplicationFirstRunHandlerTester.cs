using AltOxite.Core.Config;
using AltOxite.Core.Domain;
using AltOxite.Core.Persistence;
using NUnit.Framework;
using Rhino.Mocks;

namespace AltOxite.Tests.Config
{
    [TestFixture]
    public class DefaultApplicationFirstRunHandlerTester
    {
        private IRepository _repo;
        private DefaultApplicationFirstRunHandler _handler;
        private ISessionSourceConfiguration _config;
        private IUnitOfWork _uow;

        [SetUp]
        public void SetUp()
        {
            _repo = MockRepository.GenerateStub<IRepository>();
            _uow = MockRepository.GenerateStub<IUnitOfWork>();
            _config = MockRepository.GenerateStub<ISessionSourceConfiguration>();
            _handler = new DefaultApplicationFirstRunHandler(_config, _uow, _repo);
        }

        [Test]
        public void should_do_nothing_if_this_is_not_a_new_database()
        {
            _config.Stub(c => c.IsNewDatabase).Return(false);

            _handler.InitializeIfNecessary();

            _repo.AssertWasNotCalled(r => r.Save<User>(null), o => o.IgnoreArguments());
        }

        [Test]
        public void should_create_default_admin_user_during_initialize()
        {
            _config.Stub(c => c.IsNewDatabase).Return(true);

            var catcher = _repo.CaptureArgumentsFor(u => u.Save<User>(null));

            _handler.InitializeIfNecessary();

            var user = catcher.First<User>();

            user.Username.ShouldEqual("Admin");
            user.Password.ShouldEqual("pa$$w0rd");
        }

        [Test]
        public void should_commit_the_transaction_when_done()
        {
            _config.Stub(c => c.IsNewDatabase).Return(true);
            
            _handler.InitializeIfNecessary();

            _uow.AssertWasCalled(u => u.Commit());
        }
    }
}