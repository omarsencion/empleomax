using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EmpleosWebMax.Domain.Core.Specification
{
    public abstract class Specification<TEntity> : ISpecification<TEntity> where TEntity : BaseEntity
    {
        public abstract Expression<Func<TEntity, bool>> SatisfiedBy();

        public static Specification<TEntity> operator &(Specification<TEntity> leftSideSpecification, Specification<TEntity> rightSideSpecification) 
        {
            return new AndSpecification<TEntity>(leftSideSpecification, rightSideSpecification);
        }

        public static Specification<TEntity> operator |(Specification<TEntity> leftSideSpecification, Specification<TEntity> rightSideSpecification) 
        {
            return new OrSpecification<TEntity>(leftSideSpecification, rightSideSpecification);
        }

        public static Specification<TEntity> operator !(Specification<TEntity> specification) 
        {
            return new NotSpecification<TEntity>(specification);
        }
    }
}
