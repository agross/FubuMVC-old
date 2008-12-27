using System;
using FluentNHibernate.Framework;
using FubuMVC.Core;
using NHibernate;

namespace AltOxite.Core.Persistence
{
    public class NHibernateUnitOfWork : INHibernateUnitOfWork
    {
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
            
            CurrentSession = _source.CreateSession();
            _transaction = CurrentSession.BeginTransaction();
        }

        public ISession CurrentSession { get; private set; }

        public void SaveNew(params object[] entities)
        {
            should_not_currently_be_disposed();

            entities.Each(e => CurrentSession.Save(e));
        }

        public void UpdateExisting(params object[] entities)
        {
            should_not_currently_be_disposed();

            entities.Each(e => CurrentSession.Update(e));
        }

        public void Delete(params object[] entities)
        {
            should_not_currently_be_disposed();

            entities.Each(e => CurrentSession.Delete(e));
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
            CurrentSession.Dispose();

            _isDisposed = true;
        }
    }
}