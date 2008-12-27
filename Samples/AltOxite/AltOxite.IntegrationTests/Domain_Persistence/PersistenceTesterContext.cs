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

        [SetUp]
        public void SetUp()
        {
            var properties = new SQLiteConfiguration()
                .UseOuterJoin()
                //.ShowSql()
                .InMemory()
                .ToProperties();

            _source = new SingleConnectionSessionSourceForSQLiteInMemoryTesting(properties, new TestPersistenceModel<MAPTYPE, ENTITY>());
            _source.BuildSchema();
        }

        public PersistenceSpecification<ENTITY> Specification { get { return new PersistenceSpecification<ENTITY>(_source); } }
    }
}