using System;
using System.Linq.Expressions;

namespace AltOxite.Core.Domain
{
    public interface IDomainQuery<ENTITY>
        where ENTITY : DomainEntity
    {
        Expression<Func<ENTITY, bool>> Expression { get;  }
    }
}