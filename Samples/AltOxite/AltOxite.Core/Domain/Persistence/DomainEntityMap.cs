using FluentNHibernate.Mapping;

namespace AltOxite.Core.Domain.Persistence
{
    public class DomainEntityMap<ENTITY> : ClassMap<ENTITY>
        where ENTITY : DomainEntity
    {
        public DomainEntityMap()
        {
            Id(e => e.ID).GeneratedBy.GuidComb();
        }
    }
}