using System;

namespace AltOxite.Core.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        void Initialize();

        void SaveOrUpdate(params object[] entities);
        void Delete(params object[] entities);

        void Commit();
        void Rollback();
    }
}