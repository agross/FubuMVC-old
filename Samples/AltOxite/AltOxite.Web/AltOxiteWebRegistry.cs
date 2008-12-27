using AltOxite.Core.Config;
using AltOxite.Core.Domain;
using AltOxite.Core.Domain.Persistence;
using AltOxite.Core.Persistence;
using AltOxite.Core.Security;
using AltOxite.Core.Services;
using FluentNHibernate.Framework;
using FubuMVC.Core.Security;
using StructureMap.Attributes;
using StructureMap.Configuration.DSL;
using InMemoryRepository = AltOxite.Core.Persistence.InMemoryRepository;
using IRepository = AltOxite.Core.Persistence.IRepository;

namespace AltOxite.Web
{
    public class AltOxiteWebRegistry : Registry
    {
        protected override void configure()
        {
            ForRequestedType<ISessionSourceConfiguration>().AsSingletons()
                .TheDefault.Is.OfConcreteType<SQLiteSessionSourceConfiguration>()
                .WithCtorArg("sql_lite_db_file_name")
                    .EqualToAppSetting("AltOxite.sql_lite_db_file_name");

            ForRequestedType<ISessionSource>().AsSingletons()
                .TheDefault.Is.ConstructedBy(ctx => 
                    ctx.GetInstance<ISessionSourceConfiguration>()
                    .CreateSessionSource(new AltOxitePersistenceModel()));

            ForRequestedType<IUnitOfWork>().CacheBy(InstanceScope.Hybrid)
                .TheDefault.Is.OfConcreteType<NHibernateUnitOfWork>();

            //TODO: TEMPORARY! Until we get the full NHib stuff implemented
            ForRequestedType<IRepository>().AsSingletons().TheDefault.Is.OfConcreteType<InMemoryRepository>();

            ForRequestedType<ISecurityDataService>().TheDefault.Is.OfConcreteType<DummySecurityDataService>();
            ForRequestedType<IPrincipalFactory>().TheDefault.Is.OfConcreteType<AltOxitePrincipalFactory>();

            ForRequestedType<SiteConfiguration>()
                .AsSingletons()
                .TheDefault.Is.ConstructedBy(() =>
                    new SiteConfiguration()
                    .FromAppSetting("AltOxite.SiteConfiguration"));
        }
    }
}