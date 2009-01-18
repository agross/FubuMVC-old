using System;
using System.Linq.Expressions;

namespace VsTemplate.Core.Domain
{
    public interface IDomainQuery<ENTITY>
        where ENTITY : DomainEntity
    {
        Expression<Func<ENTITY, bool>> Expression { get;  }
    }
}