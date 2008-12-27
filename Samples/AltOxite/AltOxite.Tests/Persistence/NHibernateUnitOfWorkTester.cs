using System;
using System.Linq;
using System.Reflection;
using FluentNHibernate.Framework;
using FubuMVC.Core;
using AltOxite.Core.Persistence;
using NHibernate;
using NUnit.Framework;
using Rhino.Mocks;

namespace AltOxite.Tests.Persistence
{
    [TestFixture]
    public class NHibernateUnitOfWorkTester_CRUD_testing : NHibernateUnitOfWorkTestingContext
    {
        private readonly object _entity1 = new object();
        private readonly object _entity2 = new object();

        [Test]
        public void SaveNew_should_save_the_specified_entities_to_the_session()
        {
            _uow.SaveNew(_entity1, _entity2);

            _session.AssertWasCalled(s => s.Save(_entity1));
            _session.AssertWasCalled(s => s.Save(_entity2));
        }

        [Test]
        public void UpdateExisting_should_update_the_specified_entities_with_the_session()
        {
            _uow.UpdateExisting(_entity1, _entity2);

            _session.AssertWasCalled(s => s.Update(_entity1));
            _session.AssertWasCalled(s => s.Update(_entity2));
        }

        [Test]
        public void Delete_should_delete_the_specified_from_the_session()
        {
            _uow.Delete(_entity1, _entity2);

            _session.AssertWasCalled(s => s.Delete(_entity1));
            _session.AssertWasCalled(s => s.Delete(_entity2));            
        }

        [Test]
        public void Commit_should_commit_the_transaction()
        {
            _uow.Commit();
            _transaction.AssertWasCalled(t=>t.Commit());
        }

        [Test]
        public void Rollback_should_commit_the_transaction()
        {
            _uow.Rollback();
            _transaction.AssertWasCalled(t => t.Rollback());
        }
    }

    [TestFixture]
    public class NHibernateUnitOfWorkTester_setup_and_dispose_testing : NHibernateUnitOfWorkTestingContext
    {
        [Test]
        public void should_begin_a_new_transaction()
        {
            _session.AssertWasCalled(s => s.BeginTransaction());
        }

        [Test]
        public void dispose_should_dispose_the_transaction()
        {
            _uow.Dispose();

            _transaction.AssertWasCalled(t => t.Dispose());
        }

        [Test]
        public void dispose_should_dispose_the_session()
        {
            _uow.Dispose();

            _session.AssertWasCalled(s => s.Dispose());
        }

        [Test]
        public void extra_calls_to_dispose_should_do_nothing()
        {
            _uow.Dispose();
            _uow.Dispose();
            _uow.Dispose();

            _session.AssertWasCalled(s => s.Dispose(), o=>o.Repeat.Once());
        }

        [Test]
        public void all_other_methods_should_throw_after_the_UOW_has_been_disposed()
        {
            _uow.Dispose();

            var flags = BindingFlags.Public | BindingFlags.DeclaredOnly;
            var exception = typeof (ObjectDisposedException);

            var methods = typeof (NHibernateUnitOfWork).GetMethods(flags).Where(m => m.Name != "Dispose");
            
            methods.Each(m =>
            {
                var paramCount = m.GetParameters().Length;
                var methodParams = new object[ paramCount ];

                paramCount.IterateFromZero(idx => methodParams[idx] = null);

                exception.ShouldBeThrownBy(() => m.Invoke(_uow, methodParams), m.Name);
            });
        }
    }

    public class NHibernateUnitOfWorkTestingContext
    {
        protected ISession _session;
        protected NHibernateUnitOfWork _uow;
        protected ITransaction _transaction;
        protected ISessionSource _sessionSource;

        [SetUp]
        public void SetUp()
        {
            _sessionSource = MockRepository.GenerateStub<ISessionSource>();
            _session = MockRepository.GenerateMock<ISession>();
            _transaction = MockRepository.GenerateStub<ITransaction>();

            _sessionSource.Stub(s => s.CreateSession()).Return(_session);
            _session.Stub(s => s.BeginTransaction()).Return(_transaction);

            _uow = new NHibernateUnitOfWork(_sessionSource);
            _uow.Initialize();
        }
    }
}