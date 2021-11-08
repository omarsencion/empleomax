using System;
using System.Collections.Generic;
using System.Text;

namespace EmpleosWebMax.Domain.Core.Specification
{
   public abstract class CompositeSpecification<TEntity> : Specification<TEntity> where TEntity : BaseEntity
    {
        public abstract ISpecification<TEntity> LeftSideSpecification { get; }
        public abstract ISpecification<TEntity> RightSideSpecification { get; }
    }
}
