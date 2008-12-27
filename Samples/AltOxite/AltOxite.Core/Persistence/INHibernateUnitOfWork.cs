using NHibernate;

namespace AltOxite.Core.Persistence
{
    public interface INHibernateUnitOfWork : IUnitOfWork
    {
        ISession CurrentSession { get;}
    }
}