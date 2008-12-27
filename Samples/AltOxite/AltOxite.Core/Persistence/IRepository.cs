using System;
using System.Linq;
using AltOxite.Core.Domain;

namespace AltOxite.Core.Persistence
{
    public interface IRepository
    {
        void Save<ENTITY>(ENTITY entity)
            where ENTITY : DomainEntity;

        ENTITY Load<ENTITY>(Guid id)
            where ENTITY : DomainEntity;

        IQueryable<ENTITY> Query<ENTITY>()
            where ENTITY : DomainEntity;
    }
}