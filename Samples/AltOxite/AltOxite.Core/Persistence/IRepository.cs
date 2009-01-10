using System;
using System.Linq;
using AltOxite.Core.Domain;
using NHibernate.Linq;

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

        IQueryable<ENTITY> Query<ENTITY>(IDomainQuery<ENTITY> whereQuery)
            where ENTITY : DomainEntity;
    }

    public class NHibernateRepository : IRepository
    {
        private readonly INHibernateUnitOfWork _unitOfWork;

        public NHibernateRepository(INHibernateUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Save<ENTITY>(ENTITY entity) where ENTITY : DomainEntity
        {
            _unitOfWork.CurrentSession.SaveOrUpdate(entity);
        }

        public ENTITY Load<ENTITY>(Guid id) where ENTITY : DomainEntity
        {
            return _unitOfWork.CurrentSession.Load<ENTITY>(id);
        }

        public IQueryable<ENTITY> Query<ENTITY>() where ENTITY : DomainEntity
        {
            return _unitOfWork.CurrentSession.Linq<ENTITY>();
        }

        public IQueryable<ENTITY> Query<ENTITY>(IDomainQuery<ENTITY> whereQuery) where ENTITY : DomainEntity
        {
            return _unitOfWork.CurrentSession.Linq<ENTITY>().Where(whereQuery.Expression);
        }
    }
}