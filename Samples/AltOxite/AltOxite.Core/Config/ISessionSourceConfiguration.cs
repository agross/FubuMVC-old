using FluentNHibernate;
using FluentNHibernate.Framework;

namespace AltOxite.Core.Config
{
    public interface ISessionSourceConfiguration
    {
        ISessionSource CreateSessionSource(PersistenceModel model);
    }
}