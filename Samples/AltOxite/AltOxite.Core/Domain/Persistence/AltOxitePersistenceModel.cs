using FluentNHibernate;

namespace AltOxite.Core.Domain.Persistence
{
    public class AltOxitePersistenceModel : PersistenceModel
    {
        public AltOxitePersistenceModel()
        {
            addMappingsFromThisAssembly();
        }
    }
}