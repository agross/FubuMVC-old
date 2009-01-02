using AltOxite.Core.Domain;
using AltOxite.Core.Domain.Persistence;
using FluentNHibernate.Cfg;
using FluentNHibernate.Framework;
using NUnit.Framework;

namespace AltOxite.IntegrationTests.Domain_Persistence
{
    public class PersistenceTesterContext<MAPTYPE, ENTITY>
        where MAPTYPE : DomainEntityMap<ENTITY>, new()
        where ENTITY : DomainEntity, new()
    {
        private SingleConnectionSessionSourceForSQLiteInMemoryTesting _source;
        private TestPersistenceModel<MAPTYPE, ENTITY> _persistenceModel;

        [SetUp]
        public void SetUp()
        {
            var properties = new SQLiteConfiguration()
                .UseOuterJoin()
                //.ShowSql()
                .InMemory()
                .ToProperties();

            _persistenceModel = new TestPersistenceModel<MAPTYPE, ENTITY>();

            ReferencesAdditionalMaps(_persistenceModel);
            
            _source = new SingleConnectionSessionSourceForSQLiteInMemoryTesting(properties, _persistenceModel);
            _source.BuildSchema();
        }

        public virtual void ReferencesAdditionalMaps(TestPersistenceModel<MAPTYPE, ENTITY> model)
        {
        }

        public PersistenceSpecification<ENTITY> Specification { get { return new PersistenceSpecification<ENTITY>(_source); } }
    }
}