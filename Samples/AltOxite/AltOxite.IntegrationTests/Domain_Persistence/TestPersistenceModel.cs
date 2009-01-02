using AltOxite.Core.Domain;
using AltOxite.Core.Domain.Persistence;
using FluentNHibernate;

namespace AltOxite.IntegrationTests.Domain_Persistence
{
    public class TestPersistenceModel<MAPTYPE, ENTITY> : PersistenceModel
        where MAPTYPE : DomainEntityMap<ENTITY>, new()
        where ENTITY : DomainEntity
    {
        public TestPersistenceModel()
        {
            addMapping(new MAPTYPE());
        }

        public void IncludeMapping<OTHERMAPTYPE, OTHERENTITY>()
            where OTHERMAPTYPE : DomainEntityMap<OTHERENTITY>, new()
            where OTHERENTITY : DomainEntity
        {
            addMapping(new OTHERMAPTYPE());
        }
    }
}