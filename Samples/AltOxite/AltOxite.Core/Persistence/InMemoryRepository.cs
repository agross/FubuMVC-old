using System;
using System.Collections.Generic;
using System.Linq;
using AltOxite.Core.Domain;
using FubuMVC.Core.Util;

namespace AltOxite.Core.Persistence
{
    public class InMemoryRepository : IRepository
    {
        private readonly Cache<Type, IList<object>> _cache = new Cache<Type, IList<object>>(t=>new List<object>());

        public void Save<ENTITY>(ENTITY entity)  where ENTITY : DomainEntity
        {
            var list = _cache.Retrieve(typeof(ENTITY));

            if( ! list.Contains(entity) )
                list.Add(entity);

        }

        public ENTITY Load<ENTITY>(Guid id) where ENTITY : DomainEntity
        {
            return (ENTITY) _cache
                                .Retrieve(typeof (ENTITY))
                                .SingleOrDefault(e => e is ENTITY && ((ENTITY) e).ID == id);
        }

        public IQueryable<ENTITY> Query<ENTITY>() where ENTITY : DomainEntity
        {
            return _cache.Retrieve(typeof (ENTITY)).Cast<ENTITY>().AsQueryable();
        }
    }
}