using System;

namespace AltOxite.Core.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        void Initialize();
        void Commit();
        void Rollback();
    }
}