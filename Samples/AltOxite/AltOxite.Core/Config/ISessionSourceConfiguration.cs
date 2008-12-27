using FluentNHibernate;
using FluentNHibernate.Framework;

namespace AltOxite.Core.Config
{
    public interface ISessionSourceConfiguration
    {
        bool IsNewDatabase { get; }
        ISessionSource CreateSessionSource(PersistenceModel model);
    }
}