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
    public class NHibernateUnitOfWorkTester_transaction_testing
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

        [Test]
        public void Commit_should_commit_the_transaction()
        {
            _uow.Commit();
            _transaction.AssertWasCalled(t=>t.Commit());
        }

        [Test]
        public void Commit_should_dispose_the_transaction_and_start_a_new_transaction()
        {
            _uow.Commit();
            _transaction.AssertWasCalled(t=>t.Dispose());
            _session.AssertWasCalled(s=>s.BeginTransaction(), o=>o.Repeat.Twice());
        }

        [Test]
        public void Rollback_should_rollback_the_transaction()
        {
            _uow.Rollback();
            _transaction.AssertWasCalled(t => t.Rollback());
        }

        [Test]
        public void Rollback_should_dispose_the_transaction_and_start_a_new_transaction()
        {
            _uow.Rollback();
            _transaction.AssertWasCalled(t => t.Dispose());
            _session.AssertWasCalled(s => s.BeginTransaction(), o => o.Repeat.Twice());
        }

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

        [Test]
        public void all_other_methods_should_throw_if_not_initialized()
        {
            _uow = new NHibernateUnitOfWork(_sessionSource);

            var flags = BindingFlags.Public | BindingFlags.DeclaredOnly;
            var exception = typeof(InvalidOperationException);

            var methods = typeof(NHibernateUnitOfWork).GetMethods(flags).Where(m => m.Name != "Initialize" && m.Name != "Dispose");

            methods.Each(m =>
            {
                var paramCount = m.GetParameters().Length;
                var methodParams = new object[paramCount];

                paramCount.IterateFromZero(idx => methodParams[idx] = null);

                exception.ShouldBeThrownBy(() => m.Invoke(_uow, methodParams), m.Name);
            });
        }
    }
}