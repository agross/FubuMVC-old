using System;
using System.Linq;
using AltOxite.Core.Domain;
using AltOxite.Core.Persistence;
using NHibernate;
using NHibernate.Linq;
using NUnit.Framework;
using Rhino.Mocks;

namespace AltOxite.Tests.Persistence
{
    [TestFixture]
    public class NHibernateRepositoryTester
    {
        private INHibernateUnitOfWork _uow;
        private NHibernateRepository _repo;
        private ISession _session;

        [SetUp]
        public void SetUp()
        {
            _uow = MockRepository.GenerateMock<INHibernateUnitOfWork>();
            _session = MockRepository.GenerateMock<ISession>();
            _uow.Stub(u => u.CurrentSession).Return(_session);
            _repo = new NHibernateRepository(_uow);
        }

        [Test]
        public void Save_should_save_on_the_session()
        {
            var user = new User();
            _repo.Save(user);

            _session.AssertWasCalled(s => s.SaveOrUpdate(user));
        }

        [Test]
        public void Load_should_load_from_the_session()
        {
            var userID = Guid.NewGuid();
            _repo.Load<User>(userID);

            _session.AssertWasCalled(s => s.Load<User>(userID));
        }

        [Test]
        public void Query_should_start_a_linq_query_on_the_session()
        {
            _repo.Query<User>().ShouldBeOfType<IQueryable<User>>();
        }
    }
}