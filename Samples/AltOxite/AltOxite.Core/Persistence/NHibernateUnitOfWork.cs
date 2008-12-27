using System;
using FluentNHibernate.Framework;
using FubuMVC.Core;
using NHibernate;

namespace AltOxite.Core.Persistence
{
    public class NHibernateUnitOfWork : IUnitOfWork
    {
        private ISession _session;
        private ITransaction _transaction;
        private bool _isDisposed;
        private readonly ISessionSource _source;

        public NHibernateUnitOfWork(ISessionSource source)
        {
            _source = source;
        }

        public void Initialize()
        {
            should_not_currently_be_disposed();
            
            _session = _source.CreateSession();
            _transaction = _session.BeginTransaction();
        }

        public void SaveNew(params object[] entities)
        {
            should_not_currently_be_disposed();

            entities.Each(e => _session.Save(e));
        }

        public void UpdateExisting(params object[] entities)
        {
            should_not_currently_be_disposed();

            entities.Each(e => _session.Update(e));
        }

        public void Delete(params object[] entities)
        {
            should_not_currently_be_disposed();

            entities.Each(e => _session.Delete(e));
        }

        public void Commit()
        {
            should_not_currently_be_disposed();

            _transaction.Commit();
        }

        public void Rollback()
        {
            should_not_currently_be_disposed();

            _transaction.Rollback();
        }

        private void should_not_currently_be_disposed()
        {
            if( _isDisposed ) throw new ObjectDisposedException(GetType().Name);
        }

        public void Dispose()
        {
            if (_isDisposed) return;

            _transaction.Dispose();
            _session.Dispose();

            _isDisposed = true;
        }
    }
}